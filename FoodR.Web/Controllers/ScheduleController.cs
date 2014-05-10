using AutoMapper;
using FoodR.Data;
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
		private readonly IRepository repository;
		private readonly IScheduleService service;
		private readonly IMappingEngine mapper;

		public ScheduleController(IRepository repository, IScheduleService service, IMappingEngine mapper)
	    {
			this.repository = repository;
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
			ScheduledStopEditViewModel vm = new ScheduledStopEditViewModel();
			return View(vm);
		}

		[HttpPost]
		[Route("schedule/create/{truckurlslug}")]
		public ActionResult Create(ScheduledStopEditViewModel vm, string truckurlslug)
		{
			ScheduledStop entry = mapper.Map<ScheduledStop>(vm);

			ServiceCallResult result = service.CreateScheduledStop(entry);

			return RedirectToAction("Details", "Trucks", new { slug = truckurlslug });
		}

		[HttpGet]
		[Route("schedule/edit/{id}")]
		public ActionResult Edit(int id)
		{
			ScheduledStop entry = service.GetScheduledStopById(id);

			var vm = mapper.Map<ScheduledStopEditViewModel>(entry);
			vm.Locations = mapper.Map<IEnumerable<SelectListItem>>(repository.GetAll<Location>());
			return View(vm);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "id"), HttpPost]
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
					Entries = mapper.Map<IEnumerable<ScheduledStopDetailsViewModel>>(d.Entries)
				});
			}

			//var daysInSchedule = mapper.Map<IEnumerable<DailyScheduleViewModel>>(days);
			var vm = new WeeklyScheduleViewModel();

			vm.Days = daysInSchedule;

			return PartialView(vm);
		}
	}
}