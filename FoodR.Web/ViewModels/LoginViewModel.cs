using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security;

namespace FoodR.Web.ViewModels
{
	public class LoginViewModel
	{
		public string ReturnUrl { get; set; }
		public IEnumerable<AuthenticationDescription> LoginProviders { get; set; }
	}
}