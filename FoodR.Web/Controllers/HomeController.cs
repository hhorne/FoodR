using AutoMapper;
using FoodR.Data;
using FoodR.Data.Models;
using FoodR.Web.Services;
using FoodR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FoodR.Web.Controllers
{
    public class HomeController : FoodRController
    {
		private readonly IRepository repository;
		private readonly IScheduleService schedService;

		public HomeController(IRepository repository, IScheduleService schedService)
		{
			this.repository = repository;
			this.schedService = schedService;
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
				m.Trucks = db.FoodTrucks.Where(t => t.ScheduledStops.Any(e => e.From < myDate && e.To > myDate)).ToArray();
			}

			return View(m);
		}

		public JsonResult Json()
		{
			using (FoodRContext db = new FoodRContext())
			{
				//Find all the trucks out right now.
				//TODO: Prolly want to expand this to check for trucks that will be out with in an hour from now, maybe.
				//TESTING: this date should be changed to now, but for testing im leaving it like this
				var myDate = new DateTime(2014, 4, 23, 11, 15, 0);
				var trucks = db.FoodTrucks.Where(t => t.ScheduledStops.Any(e => e.From < myDate && e.To > myDate)).ToArray();
				return Json(trucks, JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult MapAng()
		{
			return View();
		}

		public ActionResult Menu()
		{
			return View();
		}

		public ActionResult Today()
		{
			var trucks = schedService.GetSchedulesByDay(DateTime.Now);
			return View(trucks.ToArray());
		}

		public ActionResult ListAng()
		{

			return View();
		}

		public ActionResult Locate()
		{
			return View();
		}

		public ActionResult Trouble()
		{
			return View("Error");
		}

		public ActionResult Ohsnap()
		{
			return View();
		}
	}
}