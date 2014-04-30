using FoodR.Data.Models;

namespace FoodR.Data.Configuration.EntityConfig
{
	public class BadgeConfig : EntityConfig<Badge>
	{
		public BadgeConfig()
		{
			Property(b => b.Name).HasMaxLength(150);
		}
	}
}