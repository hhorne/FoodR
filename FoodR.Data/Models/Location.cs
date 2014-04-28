using System.Collections.Generic;

namespace FoodR.Data.Models
{
	public class Location
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string UrlSlug { get; set; }
		public int AreaId { get; set; }
		public virtual Area Area { get; set; }
		public string StreetAddress { get; set; }
		public string StreetAddress2 { get; set; }
		public string ZipCode { get; set; }
	}

	public class Area
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public virtual ICollection<Location> Locations { get; set; }
		public virtual City City { get; set; }
	}
}