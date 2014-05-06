using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodR.Data.Models
{
	public class RecurringStop
	{
		public RecurringStop()
		{
			
		}

		public int FoodTruckId { get; set; }
		public virtual FoodTruck FoodTruck { get; set; }

		public int LocationId { get; set; }
		public virtual Location Location { get; set; }

		public int Id { get; set; }
		public bool Active { get; set; }
		public DateTime Starting { get; set; } //Also stores the 'from' time component
		public DateTime To { get; set; }
		
	}
}
