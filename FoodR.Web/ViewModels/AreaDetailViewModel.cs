using System.Collections.Generic;
using FoodR.Data.Models;

namespace FoodR.Web.ViewModels
{
	public class AreaDetailViewModel
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public IEnumerable<LocationDetailViewModel> Locations { get; set; }
	}

	public class LocationDetailViewModel
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public Address Address { get; set; }
	}
}