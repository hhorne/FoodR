using FoodR.Data.Models;

namespace FoodR.Data.Configuration.EntityConfig
{
	public class AreaConfig : EntityConfig<Area>
	{
		public AreaConfig()
		{
			Property(a => a.Name).HasMaxLength(150);
			Property(a => a.Description).HasMaxLength(500);
		}
	}
}