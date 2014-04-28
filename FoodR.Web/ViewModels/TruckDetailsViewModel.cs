﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodR.Web.ViewModels
{
	public class TruckDetailsViewModel
	{
		public int Id { get; set; }
		public string UrlSlug { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Facebook { get; set; }
		public string Website { get; set; }
		public string Twitter { get; set; }
		public int Rating { get; set; }

		public string Phone { get; set; }
		public string Email { get; set; }

		public TruckDetailsPageState PageState { get; set; }
		public IEnumerable<string> EditErrors { get; set; }
	}

	public enum TruckDetailsPageState
	{
		Loaded,
		SaveSuccessfully,
		SaveFailed
	}
}