using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FoodR.Web.Data.Models
{
	public class Location
	{
		[Key]
		public int LocationId { get; set; }

		public int TimesUsed { get; set; }

		public string Name { get; set; }

		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string Address3 { get; set; }
		public string Address4 { get; set; }
		public string Address5 { get; set; }

		public double Lat { get; set; }
		public double Lng { get; set; }

	}
}