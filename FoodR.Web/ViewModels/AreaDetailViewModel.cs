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
		public string StreetAddress { get; set; }
		public string StreetAddress2 { get; set; }
		public string AreaCityName { get; set; }
		public string ZipCode { get; set; }
		public string AreaCityStateId { get; set; }
		public string UrlSlug { get; set; }
	}
}