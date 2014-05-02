﻿using System.Web.Mvc;
using System.Web.Routing;

namespace FoodR.Web
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			
			routes.MapMvcAttributeRoutes();
			
			//routes.MapRoute(
			//	name: "TruckEdit",
			//	url: "Truck/edit/{truckName}",
			//	defaults: new
			//	{
			//		controller = "Trucks",
			//		action = "Edit"
			//	}
			//);
			routes.MapRoute(
				name: "Home",
				url: "",
				defaults: new { controller = "Home", action = "Today", id = UrlParameter.Optional }
			);
			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);

			
		}
	}
}