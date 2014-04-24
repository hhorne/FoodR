using FoodR.Web.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodR.Web.ViewModels
{
	public class TruckViewModel
	{
		public bool EditMode { get; set; }
		public FoodTruck Truck { get; set; }
		public IEnumerable<IGrouping<DateTime, Event>> EventsInDays { get; set; }
		//public List<EventsInDay> EventsInDays { get; set; }
	}

	public class EventsInDay
	{
		public DateTime Day { get; set; }
		public List<Event> Events { get; set; }
	}
}