using AutoMapper;
using FoodR.Data.Models;
using FoodR.Web.Services;
using FoodR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodR.Web.Controllers
{
	public class ScheduleController : FoodRController
    {
		private readonly ITruckService service;
		private readonly IMappingEngine mapper;

		public ScheduleController(ITruckService service, IMappingEngine mapper)
	    {
		    this.service = service;
		    this.mapper = mapper;
	    }

		[Route("schedule/index/{name}")]
        public ActionResult Index(string name)
        {
			if (string.IsNullOrEmpty(name))
			{
				return HttpNotFound();
			}
			WeeklyScheduleViewModel vm = new WeeklyScheduleViewModel();

			ViewBag.UrlSlug = name;
			return View(vm);
        }

		[HttpGet]
		[Route("schedule/create")]
		public ActionResult Create()
		{
			ScheduleEntryEditViewModel vm = new ScheduleEntryEditViewModel();
			return View(vm);
		}

		[HttpPost]
		[Route("schedule/create/{truckurlslug}")]
		public ActionResult Create(ScheduleEntryEditViewModel vm, string truckurlslug)
		{
			ScheduleEntry entry = mapper.Map<ScheduleEntry>(vm);

			ServiceCallResult result = service.CreateScheduleEntry(entry);

			return RedirectToAction("Details", "Trucks", new { slug = truckurlslug });
		}

		[HttpGet]
		[Route("schedule/edit/{id}")]
		public ActionResult Edit(int id)
		{
			//ScheduleEntryEditViewModel vm = new ScheduleEntryEditViewModel();
			ScheduleEntry entry = service.GetScheduleEntryById(id);

			ScheduleEntryEditViewModel vm = mapper.Map<ScheduleEntryEditViewModel>(entry);
			return View(vm);
		}

		[HttpPost]
		[Route("schedule/cancel/{id}")]
		public ActionResult Cancel(int id)
		{
			var vm = new WeeklyScheduleViewModel();
			return PartialView(vm);
		}

		[HttpGet]
		[Route("schedule/weeklyschedule/{urlslug}")]
		public ActionResult Weekly(string urlslug)
		{

			DateTime from = DateTime.Now.Date.AddDays(-1);
			DateTime to = from.AddDays(7);



			//For testing I'm leaving this without the from and to
			var days = service.GetTruckSchedule(urlslug);
			//var days = service.GetTruckSchedule(urlslug, from, to);



			List<DailyScheduleViewModel> daysInSchedule = new List<DailyScheduleViewModel>();
			foreach (ScheduleDay d in days)
			{
				daysInSchedule.Add(new DailyScheduleViewModel()
				{
					Day = d.Day,
					Entries = mapper.Map<IEnumerable<ScheduleEntryDetailsViewModel>>(d.Entries)
				});
			}

			//var daysInSchedule = mapper.Map<IEnumerable<DailyScheduleViewModel>>(days);
			var vm = new WeeklyScheduleViewModel();

			vm.Days = daysInSchedule;

			return PartialView(vm);
		}
	}
}