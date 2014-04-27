using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using FoodR.Data.Models;

namespace FoodR.Web.Controllers.Filters
{
	public abstract class SlugToIdAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var slug = filterContext.RouteData.Values["slug"] as string;
			if (slug != null)
			{
				int id = GetIdFromSlug(slug);				
				filterContext.ActionParameters["id"] = id;
			}

			base.OnActionExecuting(filterContext);
		}

		protected abstract int GetIdFromSlug(string slug);
	}
}