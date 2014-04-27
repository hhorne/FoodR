using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoodR.Data.Models;

namespace FoodR.Web.ViewModels
{
	public class LocationEditViewModel
	{
		[Required(ErrorMessage = "Provide a name for this location")]
		[MaxLength(150)]
		public string Name { get; set; }

		public Address Address { get; set; }

		public int AreaId { get; set; }
		
		public IEnumerable<SelectListItem> Areas { get; set; }
	}
}