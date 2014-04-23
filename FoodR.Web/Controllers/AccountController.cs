using FoodR.Web.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodR.Web.Controllers
{
    public class AccountController : Controller
    {
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Profile()
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