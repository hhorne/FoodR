using System.Collections.Generic;

namespace FoodR.Web.Data.Models
{
	public class Menu
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int FoodTruckId { get; set; }
		public virtual FoodTruck FoodTruck { get; set; }
		public virtual ICollection<MenuItem> MenuItems { get; set; }
	}

	public class MenuItem
	{
		public int MenuId { get; set; }
		public virtual Menu Menu { get; set; }

		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public double Price { get; set; }
	}
}