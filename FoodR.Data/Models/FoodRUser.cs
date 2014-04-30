using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FoodR.Data.Models
{
	public partial class FoodRUser : IdentityUser
	{
		public virtual ICollection<EarnedBadge> Badges { get; set; }

		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<FoodRUser> userManager)
		{
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await userManager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
			// Add custom user claims here
			return userIdentity;
		}
	}
}