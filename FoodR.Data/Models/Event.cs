using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodR.Web.Data.Models
{
	public class Event
	{
		[ForeignKey("FoodTruck")]
		public int FoodTruckId { get; set; }
		public virtual FoodTruck FoodTruck { get; set; }

		[ForeignKey("Location")]
		public int LocationId { get; set; }
		public virtual Location Location { get; set; }

		public int Id { get; set; }

		public bool Active { get; set; }

		public DateTime From { get; set; }
		public DateTime To { get; set; }
		
		public DateTime LastModifiedOn { get; set; }
		public string LastModifiedBy { get; set; }
		public string ModifiedComments { get; set; }
	}
}