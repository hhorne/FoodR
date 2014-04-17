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
            return View();
        }
	}
}