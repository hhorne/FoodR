using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FoodR.Web.ViewModels
{
	public class AddressViewModel
	{
		[Required]
		public string StreetAddress { get; set; }

		public string StreetAddress2 { get; set; }

		[Required]
		public string City { get; set; }

		[Required]
		public string State { get; set; }

		[Required]
		public string ZipCode { get; set; }
	}
}