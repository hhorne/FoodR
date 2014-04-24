using FoodR.Web.Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodR.Web.Services
{
	public class UserService : IUserService
	{
		private IAuthenticationManager authManager { get; set; }
		private FoodRUserManager userManager { get; set; }
		private FoodRRoleManager roleManager { get; set; }

		public UserService(IAuthenticationManager authManager, FoodRUserManager userManager, FoodRRoleManager roleManager)
		{
			this.authManager = authManager;
			this.userManager = userManager;
			this.roleManager = roleManager;
		}

		public IEnumerable<AuthenticationDescription> GetExternalAuthenticationTypes()
		{
			return authManager.GetExternalAuthenticationTypes();
		}

		public void SignIn(params ClaimsIdentity[] identities)
		{
			authManager.SignIn(identities);
		}

		public void SignIn(AuthenticationProperties authenticationProperties, params ClaimsIdentity[] identities)
		{
			authManager.SignIn(authenticationProperties, identities);
		}

		public void SignOut(params string[] authenticationTypes)
		{
			authManager.SignOut(authenticationTypes);
		}

		public Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
		{
			return authManager.GetExternalLoginInfoAsync();
		}

		public Task<FoodRUser> FindAsync(UserLoginInfo loginInfo)
		{
			return userManager.FindAsync(loginInfo);
		}

		public Task<bool> IsLockedOutAsync(string userId)
		{
			return userManager.IsLockedOutAsync(userId);
		}

		public Task<bool> GetTwoFactorEnabledAsync(string userId)
		{
			return userManager.GetTwoFactorEnabledAsync(userId);
		}

		public Task<bool> TwoFactorBrowserRememberedAsync(string userId)
		{
			return authManager.TwoFactorBrowserRememberedAsync(userId);
		}

		public ClaimsIdentity CreateTwoFactorRememberBrowserIdentity(string userId)
		{
			return authManager.CreateTwoFactorRememberBrowserIdentity(userId);	
		}

		public Task<IdentityResult> CreateUserAsync(FoodRUser user)
		{
			return userManager.CreateAsync(user);
		}

		public Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo loginInfo)
		{
			return userManager.AddLoginAsync(userId, loginInfo);
		}

		public Task<ClaimsIdentity> CreateIdentityAsync(FoodRUser user, string authenticationType)
		{
			return userManager.CreateIdentityAsync(user, authenticationType);
		}
		
		public async Task<SignInStatus> ExternalSignIn(ExternalLoginInfo loginInfo, bool isPersistent)
		{
			var user = await FindAsync(loginInfo.Login);
			if (user == null)
			{
				return SignInStatus.Failure;
			}
			if (await IsLockedOutAsync(user.Id))
			{
				return SignInStatus.LockedOut;
			}
			return await SignInOrTwoFactor(user, isPersistent);
		}

		private async Task<SignInStatus> SignInOrTwoFactor(FoodRUser user, bool isPersistent)
		{
			if (await GetTwoFactorEnabledAsync(user.Id) &&
				!await TwoFactorBrowserRememberedAsync(user.Id))
			{
				var identity = new ClaimsIdentity(DefaultAuthenticationTypes.TwoFactorCookie);
				identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
				SignIn(identity);
				return SignInStatus.RequiresTwoFactorAuthentication;
			}
			await SignInAsync(user, isPersistent, false);
			return SignInStatus.Success;
		}

		public async Task SignInAsync(FoodRUser user, bool isPersistent, bool rememberBrowser)
		{
			// Clear any partial cookies from external or two factor partial sign ins
			SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
			var userIdentity = await user.GenerateUserIdentityAsync(userManager);
			if (rememberBrowser)
			{
				var rememberBrowserIdentity = CreateTwoFactorRememberBrowserIdentity(user.Id);
				SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, userIdentity, rememberBrowserIdentity);
			}
			else
			{
				SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, userIdentity);
			}
		}
	}

	public interface IUserService
	{
		IEnumerable<AuthenticationDescription> GetExternalAuthenticationTypes();
		void SignIn(params ClaimsIdentity[] identities);
		void SignIn(AuthenticationProperties authenticationProperties, params ClaimsIdentity[] identities);
		void SignOut(params string[] authenticationTypes);
		Task<ExternalLoginInfo> GetExternalLoginInfoAsync();
		Task<FoodRUser> FindAsync(UserLoginInfo loginInfo);
		Task<bool> IsLockedOutAsync(string userId);
		Task<bool> GetTwoFactorEnabledAsync(string userId);
		Task<bool> TwoFactorBrowserRememberedAsync(string userId);
		ClaimsIdentity CreateTwoFactorRememberBrowserIdentity(string userId);
		Task<IdentityResult> CreateUserAsync(FoodRUser user);
		Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo loginInfo);
		Task<ClaimsIdentity> CreateIdentityAsync(FoodRUser user, string authenticationType);
		Task<SignInStatus> ExternalSignIn(ExternalLoginInfo loginInfo, bool isPersistent);
		Task SignInAsync(FoodRUser user, bool isPersistent, bool rememberBrowser);
	}

	public enum SignInStatus
	{
		Success,
		LockedOut,
		RequiresTwoFactorAuthentication,
		Failure
	}
}