using FoodR.Web.Data;
using FoodR.Web.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
			FoodRContext db = new FoodRContext();
			foreach(FoodTruck ft in db.FoodTrucks)
			{
				var name = ft.Name;
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

			return View();
		}

		public ActionResult Locate()
		{
			return View();
		}

		public ActionResult Truck()
		{
			FoodTruck m = new FoodTruck();
			m.Id = 1;
			m.Name = "Happy Grilled Cheese";
			m.Comments = "Some Comments from the truck";
			m.Rating = 2;
			m.Phone = "(904) 555-5556";
			m.Website = "http://www.google.com";
			return View(m);
		}
	}
}