using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodR.Web.Data.Models
{
	public class Badge
	{
		public int BadgeId { get; set; }
		public string Name { get; set; }
		public string Icon { get; set; }
	}

	public class EarnedBadge
	{
		public int EarnedBadgeId { get; set; }
		public DateTime DateEarned { get; set; }
		public int BadgeId { get; set; }
		public virtual Badge Badge { get; set; }
		public virtual FoodRUser User { get; set; }
	}
}