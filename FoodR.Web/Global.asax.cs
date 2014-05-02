using System;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;

namespace FoodR.Web
{
	public class Global : System.Web.HttpApplication
	{
		protected void Application_Start(object sender, EventArgs e)
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			TypeMapConfig.RegisterTypeMaps();
			log4net.Config.XmlConfigurator.Configure();
		}
	}
}