using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodR.Data.Models
{
	public class ScheduledStop
	{
		public ScheduledStop()
		{
			LastModifiedOn = DateTime.Now;
			//Exceptions = new List<RecurringException>().ToArray();
		}

		public int FoodTruckId { get; set; }
		public virtual FoodTruck FoodTruck { get; set; }

		public int LocationId { get; set; }
		public virtual Location Location { get; set; }

		public int Id { get; set; }

		public bool Active { get; set; }
		public bool Canceled { get; set; }
		public bool Deleted { get; set; }

		public DateTime From { get; set; }
		public DateTime To { get; set; }

		public bool Recurring { get; set; }
		public DateTime? RecurringStart { get; set; }
		public DateTime? RecurringEnd { get; set; }
		public bool HasRecurringEnd { get; set; }

		public virtual IEnumerable<RecurringException> Exceptions { get; set; }
		
		public DateTime LastModifiedOn { get; set; }
		public string LastModifiedBy { get; set; }
		public string ModifiedComments { get; set; }
	}
}