using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FoodR.Data.Models;

namespace FoodR.Web.ViewModels
{
	public class WeeklyScheduleViewModel
	{
		public IEnumerable<DailyScheduleViewModel> Days { get; set; }
	}

	public class DailyScheduleViewModel
	{
		public DateTime Day { get; set; }
		public IEnumerable<ScheduledStopDetailsViewModel> Stops { get; set; }
	}
}