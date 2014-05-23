using System.Web.Mvc;

namespace FoodR.Web
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			//This was block unhandled errors going to Elmah
			//filters.Add(new HandleErrorAttribute());
		}
	}
}