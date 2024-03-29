﻿using System.Web.Mvc;
using System.Web.Routing;

namespace FoodR.Web
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("elmah.axd");
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			
			routes.MapMvcAttributeRoutes();
			
			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Today", id = UrlParameter.Optional }
			);

			
		}
	}
}