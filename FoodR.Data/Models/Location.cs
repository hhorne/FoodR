using System.Collections.Generic;

namespace FoodR.Data.Models
{
	public class Location
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string UrlSlug { get; set; }
		public Address Address { get; set; }
		public virtual Area Area { get; set; }
	}

	public class Area
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public virtual ICollection<Location> Locations { get; set; }
	}
}