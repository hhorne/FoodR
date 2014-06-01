using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodR.Web.ViewModels
{
	public class ScheduledStopEditViewModel
	{
		public ScheduledStopEditViewModel()
		{
			HasRecurringEnd = false;
		}

		public DateTime? Day { get; set; }

		public string SelectedFrom { get; set; }
		public IEnumerable<SelectListItem> From { get; set; }

		public string SelectedTo { get; set; }
		public IEnumerable<SelectListItem> To { get; set; }

		public int LocationId { get; set; }
		public IEnumerable<SelectListItem> Locations { get; set; }

		[Editable(allowEdit: false)]
		public DateTime LastModifiedOn { get; set; }

		[MaxLength(500)]
		public string ModifiedComments { get; set; }

		public bool Recurring { get; set; }

		public DateTime? RecurringStart { get; set; }
		public bool HasRecurringEnd { get; set; }
		public DateTime? RecurringEnd { get; set; }
	}
}