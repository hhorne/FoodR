using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodR.Data;
using FoodR.Data.Models;
using FoodR.Web.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace FoodR.Web
{
	public class FoodRUserManager : UserManager<FoodRUser>
	{
		public FoodRUserManager(IUserStore<FoodRUser> store) : base(store) { }

		public static FoodRUserManager Create(IdentityFactoryOptions<FoodRUserManager> options, IOwinContext context)
		{
			var manager = new FoodRUserManager(new UserStore<FoodRUser>(context.Get<FoodRContext>()));

			// Configure validation logic for usernames
			manager.UserValidator = new UserValidator<FoodRUser>(manager)
			{ 
				AllowOnlyAlphanumericUserNames = false,
				RequireUniqueEmail = true
			};
			
			// Configure validation logic for passwords
			manager.PasswordValidator = new PasswordValidator
			{
				RequiredLength = 6,
				RequireNonLetterOrDigit = true,
				RequireDigit = true,
				RequireLowercase = true,
				RequireUppercase = true,
			};

			// Configure user lockout defaults
			manager.UserLockoutEnabledByDefault = true;
			manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
			manager.MaxFailedAccessAttemptsBeforeLockout = 5;

			// Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
			// You can write your own provider and plug in here.
			manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<FoodRUser>
			{
				MessageFormat = "Your security code is: {0}"
			});
			
			manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<FoodRUser>
			{
				Subject = "SecurityCode",
				BodyFormat = "Your security code is {0}"
			});

			manager.EmailService = new EmailService();
			manager.SmsService = new SmsService();

			var dataProtectionProvider = options.DataProtectionProvider;
			if (dataProtectionProvider != null)
			{
				manager.UserTokenProvider = new DataProtectorTokenProvider<FoodRUser>(dataProtectionProvider.Create("ASP.NET Identity"));
			}
			return manager;
		}

		public virtual async Task<IdentityResult> AddUserToRolesAsync(string userId, IList<string> roles)
		{
			var userRoleStore = (IUserRoleStore<FoodRUser, string>)Store;

			var user = await FindByIdAsync(userId).ConfigureAwait(false);
			if (user == null)
			{
				throw new InvalidOperationException("Invalid user Id");
			}

			var userRoles = await userRoleStore.GetRolesAsync(user).ConfigureAwait(false);
			// Add user to each role using UserRoleStore
			foreach (var role in roles.Where(role => !userRoles.Contains(role)))
			{
				await userRoleStore.AddToRoleAsync(user, role).ConfigureAwait(false);
			}

			// Call update once when all roles are added
			return await UpdateAsync(user).ConfigureAwait(false);
		}

		public virtual async Task<IdentityResult> RemoveUserFromRolesAsync(string userId, IList<string> roles)
		{
			var userRoleStore = (IUserRoleStore<FoodRUser, string>)Store;

			var user = await FindByIdAsync(userId).ConfigureAwait(false);
			if (user == null)
			{
				throw new InvalidOperationException("Invalid user Id");
			}

			var userRoles = await userRoleStore.GetRolesAsync(user).ConfigureAwait(false);
			// Remove user to each role using UserRoleStore
			foreach (var role in roles.Where(userRoles.Contains))
			{
				await userRoleStore.RemoveFromRoleAsync(user, role).ConfigureAwait(false);
			}

			// Call update once when all roles are removed
			return await UpdateAsync(user).ConfigureAwait(false);
		}
	}

	public class FoodRUserStore : UserStore<FoodRUser>
	{
		public FoodRUserStore() : base(new FoodRContext()) { }
	}

	public class FoodRRoleManager : RoleManager<IdentityRole>
	{
		public FoodRRoleManager(IRoleStore<IdentityRole, string> roleStore) : base(roleStore) { }

		public static FoodRRoleManager Create(IdentityFactoryOptions<FoodRRoleManager> options, IOwinContext context)
		{
			var manager = new FoodRRoleManager(new RoleStore<IdentityRole>(context.Get<FoodRContext>()));
			return manager;
		}
	}

	public class FoodRRoleStore : RoleStore<IdentityRole, string, IdentityUserRole>
	{
		public FoodRRoleStore() : base (new FoodRContext()) { }
	}
}