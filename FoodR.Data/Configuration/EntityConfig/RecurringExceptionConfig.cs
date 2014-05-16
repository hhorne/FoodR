using FoodR.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodR.Data.Configuration.EntityConfig
{
	public class RecurringExceptionConfig : EntityConfig<RecurringException>
	{
		public RecurringExceptionConfig()
		{
			HasRequired(s => s.ScheduledStop).
				WithMany().
				HasForeignKey(s => s.ScheduledStopId).
				WillCascadeOnDelete(false);
		}
	}
}
