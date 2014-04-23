using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FoodR.Web.ViewModels;
using Microsoft.Owin.Security;

namespace FoodR.Web.Controllers
{
	public class AuthController : FoodRController
	{
		private readonly IAuthenticationManager authManager;

		public AuthController(IAuthenticationManager authManager)
		{
			this.authManager = authManager;			
		}

		public ActionResult Login(string returnUrl)
		{
			var viewModel = new LoginViewModel
			{
				ReturnUrl = returnUrl,
				LoginProviders = authManager.GetExternalAuthenticationTypes() 
			};

			return View(viewModel);
		}
	}
}