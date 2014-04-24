using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FoodR.Web.Data.Models;

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

		public ActionResult Manage()
		{
			FoodTruck m = new FoodTruck();
			m.Id = 1;
			m.Name = "Happy Grilled Cheese";

			return View(m);
		}
	}
}