using FoodR.Web.Data.Models;

namespace FoodR.Web.Data.EntityTypeConfiguration
{
	public class BadgeConfig : EntityConfig<Badge>
	{
		public BadgeConfig()
		{
			Property(b => b.Name).HasMaxLength(150);
		}
	}
}