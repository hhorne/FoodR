using System.Linq;
using FoodR.Data;

namespace FoodR.Web.Controllers.Filters
{
	public class TruckSlutToIdAttribute : SlugToIdAttribute
	{
		protected override int GetIdFromSlug(string slug)
		{
			using (var context = new FoodRContext())
			{
				var result = context.FoodTrucks.SingleOrDefault(t => t.UrlSlug == slug);
				if (result == null)
				{
					return 0;
				}

				return result.Id;
			}
		}
	}
}