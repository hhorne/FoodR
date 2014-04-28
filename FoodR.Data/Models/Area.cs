using System.Collections.Generic;

namespace FoodR.Data.Models
{
	public class Area
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public virtual ICollection<Location> Locations { get; set; }
		public virtual City City { get; set; }
	}
}