using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace FoodR.Web.Controllers.Filters
{
	public class UserAwareAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			filterContext.Controller.ViewBag.UserName = GetCurrentUserName();
			filterContext.Controller.ViewBag.IsAdmin = GetCurrentUsersRoles();
			base.OnActionExecuted(filterContext);
		}

		public bool GetCurrentUsersRoles()
		{
			if (HttpContext.Current.User.Identity.IsAuthenticated)
			{
				return HttpContext.Current.User.IsInRole("Admin");
			}

			return false;
		}

		public string GetCurrentUserName()
		{
			if (HttpContext.Current.User.Identity.IsAuthenticated)
			{
				return HttpContext.Current.User.Identity.Name;
			}

			return "Anonymous";
		}
	}
}