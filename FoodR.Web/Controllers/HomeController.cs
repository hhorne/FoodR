using FoodR.Web.Data;
using FoodR.Web.Data.Models;
using FoodR.Web.ViewModels;
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
			using (FoodRContext db = new FoodRContext())
			{
				foreach (FoodTruck ft in db.FoodTrucks)
				{
					var name = ft.Name;
				}
			}
            return View();
        }

		public ActionResult Map()
		{
			List<FoodTruck> vendors = new List<FoodTruck>();

			vendors.Add(new FoodTruck() { 
				Id = 1,
				Name = "Grannys Chicken"
			});

			MapModel m = new MapModel();
			m.Vendors = vendors.ToArray();

			

			return View(m);
		}

		public ActionResult Schedule()
		{
			FoodRContext db = new FoodRContext();
			return View(db.FoodTrucks.ToArray());
		}

		public ActionResult ScheduleAng()
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