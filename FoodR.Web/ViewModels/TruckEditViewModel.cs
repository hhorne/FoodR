using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace FoodR.Web.ViewModels
{
	public class TruckEditViewModel
	{
		[Required(ErrorMessage = "Provide a name for this truck")]
		[MaxLength(150)]
		public string Name { get; set; }

		[Required(ErrorMessage = "Provide a description for this truck")]
		[MaxLength(1000)]
		public string Description { get; set; }

		[MaxLength(30)]
		public string Phone { get; set; }

		[MaxLength(50)]
		public string Email { get; set; }

		[MaxLength(200)]
		public string Website { get; set; }

		[MaxLength(50)]
		public string Facebook { get; set; }

		[MaxLength(50)]
		public string Twitter { get; set; }

		//public int Rating { get; set; }

		[ScaffoldColumn(false)]
		public TruckDetailsPageState PageState { get; set; }

		[ScaffoldColumn(false)]
		public IEnumerable<string> EditErrors { get; set; }

		public IEnumerable<SelectListItem> Categories { get; set; }

		[ScaffoldColumn(false)]
		public int CategoryId { get; set; }
	}

	public enum TruckDetailsPageState
	{
		Loaded,
		SaveSuccessfully,
		SaveFailed
	}
}