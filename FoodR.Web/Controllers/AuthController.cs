﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FoodR.Web.Data.Models;
using FoodR.Web.Services;
using FoodR.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace FoodR.Web.Controllers
{	
	public class AuthController : FoodRController
	{
		private readonly IUserService userService;

		public AuthController(IUserService userService)
		{
			this.userService = userService;
		}

		public ActionResult Login(string returnUrl)
		{
			var viewModel = new LoginViewModel
			{
				ReturnUrl = returnUrl,
				LoginProviders = userService.GetExternalAuthenticationTypes() 
			};

			return View(viewModel);
		}

		[HttpPost]
		public ActionResult Logout()
		{
			userService.SignOut();
			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult ExternalLogin(string provider, string returnUrl)
		{
			// Request a redirect to the external login provider
			return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Auth", new { ReturnUrl = returnUrl }));
		}

		public enum SignInStatus
		{
			Success,
			LockedOut,
			RequiresTwoFactorAuthentication,
			Failure
		}

		[AllowAnonymous]
		public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
		{
			var loginInfo = await userService.GetExternalLoginInfoAsync();
			if (loginInfo == null)
			{
				return RedirectToAction("Login");
			}

			// Sign in the user with this external login provider if the user already has a login
			var result = await ExternalSignIn(loginInfo, isPersistent: false);
			switch (result)
			{
				case SignInStatus.Success:
					return RedirectToLocal(returnUrl);
				case SignInStatus.LockedOut:
					return View("Lockout");
				case SignInStatus.RequiresTwoFactorAuthentication:
					return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
				case SignInStatus.Failure:
				default:
					// If the user does not have an account, then prompt the user to create an account
					ViewBag.ReturnUrl = returnUrl;
					ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
					return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
			}
		}

		[NonAction]
		public async Task<SignInStatus> ExternalSignIn(ExternalLoginInfo loginInfo, bool isPersistent)
		{
			var user = await userService.FindAsync(loginInfo.Login);
			if (user == null)
			{
				return SignInStatus.Failure;
			}
			if (await userService.IsLockedOutAsync(user.Id))
			{
				return SignInStatus.LockedOut;
			}
			return await SignInOrTwoFactor(user, isPersistent);
		}

		[NonAction]
		private async Task<SignInStatus> SignInOrTwoFactor(FoodRUser user, bool isPersistent)
		{
			if (await userService.GetTwoFactorEnabledAsync(user.Id) &&
				!await userService.TwoFactorBrowserRememberedAsync(user.Id))
			{
				var identity = new ClaimsIdentity(DefaultAuthenticationTypes.TwoFactorCookie);
				identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
				userService.SignIn(identity);
				return SignInStatus.RequiresTwoFactorAuthentication;
			}
			await SignInAsync(user, isPersistent, false);
			return SignInStatus.Success;
		}

		public async Task SignInAsync(FoodRUser user, bool isPersistent, bool rememberBrowser)
		{
			// Clear any partial cookies from external or two factor partial sign ins
			userService.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
			var userIdentity = await user.GenerateUserIdentityAsync(userService);
			if (rememberBrowser)
			{
				var rememberBrowserIdentity = userService.CreateTwoFactorRememberBrowserIdentity(user.Id);
				userService.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, userIdentity, rememberBrowserIdentity);
			}
			else
			{
				userService.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, userIdentity);
			}
		}

		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
		{
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "Manage");
			}

			if (ModelState.IsValid)
			{
				// Get the information about the user from the external login provider
				var info = await userService.GetExternalLoginInfoAsync();
				if (info == null)
				{
					return View("ExternalLoginFailure");
				}
				var user = new FoodRUser { UserName = model.Email, Email = model.Email };
				var result = await userService.CreateUserAsync(user);
				if (result.Succeeded)
				{
					result = await userService.AddLoginAsync(user.Id, info.Login);
					if (result.Succeeded)
					{
						await SignInAsync(user, isPersistent: false, rememberBrowser: false);
						return RedirectToLocal(returnUrl);
					}
				}
				AddErrors(result);
			}

			ViewBag.ReturnUrl = returnUrl;
			return View(model);
		}

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error);
			}
		}

		public class ExternalLoginConfirmationViewModel
		{
			[Required]
			[Display(Name = "Email")]
			public string Email { get; set; }
		}
	}
}