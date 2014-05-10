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
		[DataType(DataType.DateTime)]
		[Required(ErrorMessage = "Provide a From date")]
		public DateTime From { get; set; }

		[DataType(DataType.DateTime)]
		[Required(ErrorMessage = "Provide a To date")]
		public DateTime To { get; set; }

		[ScaffoldColumn(false)]
		public int LocationId { get; set; }

		public IEnumerable<SelectListItem> Locations { get; set; }

		[Editable(allowEdit: false)]
		public DateTime LastModifiedOn { get; set; }

		[MaxLength(500)]
		public string ModifiedComments { get; set; }

		public bool Recurring { get; set; }
	}
}