using FoodR.Web.Data;
using FoodR.Web.Data.Models;
using FoodR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FoodR.Web.Controllers
{
    public class HomeController : FoodRController
    {
		public HomeController()
		{
		}

        public ActionResult Index()
        {
            return View();
        }

		public ActionResult Map()
		{
			MapViewModel m = new MapViewModel();

			using (FoodRContext db = new FoodRContext())
			{
				//Find all the trucks out right now.
				//TODO: Prolly want to expand this to check for trucks that will be out with in an hour from now, maybe.
				//TESTING: this date should be changed to now, but for testing im leaving it like this
				var myDate = new DateTime(2014, 4, 23, 11, 15, 0);
				m.Trucks = db.FoodTrucks.Where(t => t.Events.Any(e => e.From < myDate && e.To > myDate)).ToArray();
			}

			return View(m);
		}

		public ActionResult MapAng()
		{
			return View();
		}

		public ActionResult Menu()
		{
			return View();
		}

		public ActionResult List()
		{
			using(FoodRContext db = new FoodRContext())
			{
				var today = new DateTime(2014, 4, 23);
				var tomorrow = new DateTime(2014, 4, 24);
				var trucks = db.FoodTrucks
					.Include("Events.Location")
					.Include("Menus")
					.Where(t => t.Events.Any(e => e.From >= today && e.From < tomorrow)).ToArray();
				return View(trucks.ToArray());
			}
		}

		public ActionResult ListAng()
		{

			return View();
		}

		public ActionResult Locate()
		{
			return View();
		}

		public ActionResult Truck(int id = 0)
		{
			if(id == 0) //dont wanna catch the page in an invalid state
				return RedirectToAction("Index", "Home");

			TruckViewModel m = new TruckViewModel();
			using (FoodRContext db = new FoodRContext())
			{
				//eagerly load the events and locations
				m.Truck = db.FoodTrucks
					.Include("Events.Location")
					.FirstOrDefault(t => t.Id == id);

				if (m.Truck == null) //non existent truck id
					return RedirectToAction("Index", "Home");

				m.EventsInDays = m.Truck.Events.GroupBy(e => e.From.Date);
			}

			m.EditMode = false;
			
			return View(m);
		}
	}
}