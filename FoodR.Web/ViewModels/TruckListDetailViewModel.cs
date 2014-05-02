using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodR.Web.ViewModels
{
	public class TruckListDetailViewModel
	{
		public string UrlSlug { get; set; }

		public string Name { get; set; }
		public string Description { get; set; }
		public int Rating { get; set; }
	}
}