﻿using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FoodR.Web.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FoodR.Web.Data.Models
{
	public class FoodRUser : IdentityUser
	{
		public virtual ICollection<EarnedBadge> Badges { get; set; }

		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(IUserService userService)
		{
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await userService.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
			// Add custom user claims here
			return userIdentity;
		}

		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(FoodRUserManager userManager)
		{
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await userManager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
			// Add custom user claims here
			return userIdentity;
		}
	}
}