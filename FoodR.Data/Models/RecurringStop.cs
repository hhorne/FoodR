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
		public bool HasEnding { get; set; }
		//starting date
		public DateTime Starting { get; set; } //Also stores the 'from' time component
		//the ending date, if HasEnding is true, alother wise the recurring stop goes on forever
		public DateTime Ending { get; set; } //Also stores the 'to' time compenent

		public virtual IEnumerable<RecurringStopException> Exceptions { get; set; }
	}
}
