using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodR.Web.Controllers
{
    public class ScheduleController : Controller
    {
        //
        // GET: /Schedule/
        public ActionResult Index()
        {
            return View();
        }

		[HttpGet]
		public ActionResult Create()
		{
			return View();
		}

		[HttpGet]
		public ActionResult Edit()
		{
			return View();
		}
	}
}