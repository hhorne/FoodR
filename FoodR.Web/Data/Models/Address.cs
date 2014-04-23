using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodR.Web.Data.Models
{
	public class Address
	{
		public string StreetAddress { get; set; }
		public string StreetAddress2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }
	}
}