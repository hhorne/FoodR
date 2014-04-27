using System.Web.Mvc;
using System.Web.Routing;

namespace FoodR.Web
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			
			routes.MapMvcAttributeRoutes();
			
			routes.MapRoute(
				name: "TruckEdit",
				url: "Trucks/edit/{truckName}",
				defaults: new
				{
					controller = "Trucks",
					action = "Edit"
				}
			);

			routes.MapRoute(
				name: "TruckDetails",
				url: "Trucks/{truckName}",
				defaults: new
				{
					controller = "Trucks",
					action = "Details"
				}
			);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}