using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodR.Data.Models
{
	public class RecurringException
	{
		public int Id { get; set; }

		public DateTime Day { get; set; }

		public int ScheduledStopId { get; set; }
		public virtual ScheduledStop ScheduledStop { get; set; }
	}
}
