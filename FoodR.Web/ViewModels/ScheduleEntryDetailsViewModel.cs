using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodR.Web.ViewModels
{
	public class ScheduleEntryDetailsViewModel
	{
		public DateTime From { get; set; }
		public DateTime To { get; set; }
		public DateTime LastModifiedOn { get; set; }
		public string ModifiedComments { get; set; }

		public LocationDetailViewModel Location { get; set; }
	}
}