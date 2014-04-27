using System.Collections.Generic;
using FoodR.Data.Models;

namespace FoodR.Web.ViewModels
{
	public class AreaDetailViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public IEnumerable<LocationDetailViewModel> Locations { get; set; }
	}

	public class LocationDetailViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string UrlSlug { get; set; }
		public Address Address { get; set; }
	}
}