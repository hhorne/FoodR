using FoodR.Data.Models;

namespace FoodR.Data.Configuration.EntityConfig
{
	public class LocationConfig : EntityConfig<Location>
	{
		public LocationConfig()
		{
			Property(l => l.Name).HasMaxLength(150);
		}
	}
}