using FoodR.Data.Models;

namespace FoodR.Data.Configuration.EntityConfig
{
	class CityConfig : EntityConfig<City>
	{
		public CityConfig()
		{
			Property(c => c.Name).HasMaxLength(100);
		}
	}
}
