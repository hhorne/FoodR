using FoodR.Data.Models;

namespace FoodR.Data.Configuration.EntityConfig
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