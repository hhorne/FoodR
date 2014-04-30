using FoodR.Data;
using FoodR.Data.Models;
using FoodR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodR.Web.Controllers
{
    public class ProfileController : FoodRController
    {
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Badges()
		{
			return View();
		}

		public ActionResult Manage(int id = 0)
		{
			if (id == 0) //dont wanna catch the page in an invalid state
				return RedirectToAction("Index", "Profile");

			TruckViewModel m = new TruckViewModel();
			using (FoodRContext db = new FoodRContext())
			{
				//eagerly load the events and locations
				m.Truck = db.FoodTrucks
					.Include("Events.Location")
					.FirstOrDefault(t => t.Id == id);

				if (m.Truck == null) //non existent truck id
					return RedirectToAction("Index", "Profile");

				m.EventsInDays = m.Truck.Events.GroupBy(e => e.From.Date);
			}

			m.EditMode = true;

			
			return View(m);
		}

		[HttpPost]
		public ActionResult Manage(TruckViewModel model, int id)
		{
			using(FoodRContext db = new FoodRContext())
			{
				FoodTruck truck = db.FoodTrucks
					.FirstOrDefault(t => t.Id == id);

				if(truck == null)
					return View(model);

				truck.Name = model.Truck.Name;
				truck.Description = model.Truck.Description;
				truck.Website = model.Truck.Website;

				db.SaveChanges();
			}
			
			return RedirectToAction("Manage", "Profile", new {id = id});
		}
	}
}