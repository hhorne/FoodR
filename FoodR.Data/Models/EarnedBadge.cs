using System;

namespace FoodR.Data.Models
{
	public class EarnedBadge
	{
		public int Id { get; set; }
		public DateTime DateEarned { get; set; }
		public int BadgeId { get; set; }
		public virtual Badge Badge { get; set; }
		public virtual FoodRUser User { get; set; }
	}
}