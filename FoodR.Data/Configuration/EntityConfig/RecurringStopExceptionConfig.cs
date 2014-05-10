using FoodR.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodR.Data.Configuration.EntityConfig
{
	public class RecurringStopExceptionConfig : EntityConfig<RecurringStopException>
	{
		public RecurringStopExceptionConfig()
		{
			HasRequired(s => s.RecurringStop).
				WithMany().
				HasForeignKey(s => s.RecurringStopId).
				WillCascadeOnDelete(false);

			//HasRequired(s => s.ScheduledStop).
			//	WithMany().
			//	HasForeignKey(s => s.ScheduledStopId).
			//	WillCascadeOnDelete(false);
		}
	}
}
