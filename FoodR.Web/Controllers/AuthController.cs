using FoodR.Data.Models;
using FoodR.Web.Services;
using FoodR.Web.ViewModels;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Mvc;

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

		[AllowAnonymous]
		public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
		{
			var loginInfo = await userService.GetExternalLoginInfoAsync();
			if (loginInfo == null)
			{
				return RedirectToAction("Login");
			}

			// Sign in the user with this external login provider if the user already has a login
			var result = await userService.ExternalSignIn(loginInfo, isPersistent: false);
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
						await userService.SignInAsync(user, isPersistent: false, rememberBrowser: false);
						return RedirectToLocal(returnUrl);
					}
				}

				AddIdentityErrors(result);
			}

			ViewBag.ReturnUrl = returnUrl;
			return View(model);
		}

		[NonAction]
		private void AddIdentityErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error);
			}
		}
	}
}