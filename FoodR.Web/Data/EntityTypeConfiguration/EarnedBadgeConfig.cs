using FoodR.Web.Data.Models;

namespace FoodR.Web.Data.EntityTypeConfiguration
{
	public class EarnedBadgeConfig : EntityConfig<EarnedBadge>
	{
		public EarnedBadgeConfig()
		{
			HasRequired(e => e.Badge)
				.WithMany()
				.HasForeignKey(e => e.BadgeId);

			HasRequired(e => e.User)
				.WithMany();
		}
	}
}