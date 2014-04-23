using FoodR.Web.Data.Models;

namespace FoodR.Web.Data.EntityTypeConfiguration
{
	public class LocationConfig : EntityConfig<Location>
	{
		public LocationConfig()
		{
			Property(l => l.Name).HasMaxLength(150);
		}
	}
}