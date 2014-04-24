using System.Web;
using System.Web.Mvc;

namespace FoodR.Web.Controllers.Filters
{
	public class UserAwareAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			filterContext.Controller.ViewBag.UserName = GetCurrentUserName();
			base.OnActionExecuted(filterContext);
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