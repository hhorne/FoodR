using System;
using System.Collections.Generic;

namespace FoodR.Data.Models
{
	public class Category
	{
		public Category()
		{
			CreatedOn = DateTime.Now;
			LastModifiedOn = DateTime.Now;
		}

		//public int ParentId { get; set; }
		public virtual ICollection<FoodTruck> FoodTrucks { get; set; }

		public string CreatedBy { get; set; }

		public DateTime CreatedOn { get; set; }

		public int Id { get; set; }

		public string LastModifiedBy { get; set; }

		public DateTime LastModifiedOn { get; set; }

		public string Name { get; set; }
	}
}