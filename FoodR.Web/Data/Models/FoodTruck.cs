using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodR.Web.Data.Models
{
	public class FoodTruck
	{
		public int FoodTruckId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Comments { get; set; }

		//contact info
		public string Phone { get; set; }
		public string Phone2 { get; set; }
		public string Email { get; set; }
		public string Email2 { get; set; }
		public string Website { get; set; }
		public string Facebook { get; set; }
		public string Twitter { get; set; }

		public DateTime LastModifiedOn { get; set; }
		public virtual FoodRUser LastModifiedBy { get; set; }

		public DateTime CreatedOn { get; set; }
		public virtual FoodRUser CreatedBy { get; set; }
	}
}