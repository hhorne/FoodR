using AutoMapper;
using FoodR.Data;
using FoodR.Data.Models;
using FoodR.Web.Services;
using FoodR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace FoodR.Web.Controllers
{
	public class ScheduleController : FoodRController
    {
		private readonly IRepository repository;
		private readonly IScheduleService schedService;
		private readonly ITruckService truckService;
		private readonly IMappingEngine mapper;

		public ScheduleController(IRepository repository, IScheduleService schedService, ITruckService truckService, IMappingEngine mapper)
	    {
			this.repository = repository;
			this.schedService = schedService;
			this.truckService = truckService;
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "truckurlslug"), HttpGet]
		[Route("schedule/create/{truckurlslug}")]
		public ActionResult Create(string truckurlslug)
		{
			ScheduledStopEditViewModel vm = new ScheduledStopEditViewModel();
			var locations = schedService.GetLocations();
			vm.Locations = mapper.Map<IEnumerable<SelectListItem>>(locations);

			var timeSlots = CreateTimeSlots();
			vm.From = timeSlots;
			vm.To = timeSlots;

			return View(vm);
		}

		private IEnumerable<SelectListItem> CreateTimeSlots()
		{
			List<SelectListItem> timeSlots = new List<SelectListItem>();
			int hour = 10;
			string ampm = "AM";
			bool go = true;
			while (go)
			{
				if (hour == 2 && ampm == "AM")
					go = false;

				if(hour == 12)
					ampm = ampm == "AM" ? "PM" : "AM";

				timeSlots.Add(new SelectListItem()
				{
					Text = string.Format("{0}:00 {1}", hour, ampm)
				});
				timeSlots.Add(new SelectListItem()
				{
					Text = string.Format("{0}:30 {1}", hour, ampm)
				});

				hour++;
				if(hour > 12)
				{
					hour = 1;
					
				}
			}
			
			return timeSlots.ToArray();
		}

		[HttpPost]
		[Route("schedule/create/{truckurlslug}")]
		public ActionResult Create(ScheduledStopEditViewModel vm, string truckurlslug)
		{
			var stop = mapper.Map<ScheduledStop>(vm);
			DateTime from;
			DateTime to;
			DateTime.TryParseExact(vm.SelectedFrom, @"h\:mm tt", CultureInfo.CurrentCulture, DateTimeStyles.None, out from);
			DateTime.TryParseExact(vm.SelectedTo, @"h\:mm tt", CultureInfo.CurrentCulture, DateTimeStyles.None, out to);

			stop.From = new DateTime(vm.Day.Value.Year,vm.Day.Value.Month, vm.Day.Value.Day, from.Hour, from.Minute, from.Second);
			stop.To = new DateTime(vm.Day.Value.Year, vm.Day.Value.Month, vm.Day.Value.Day, to.Hour, to.Minute, to.Second);
			stop.LastModifiedOn = DateTime.Now;

			var truck = truckService.GetTruckByUrl(truckurlslug);
			stop.FoodTruckId = truck.Id;
			schedService.CreateStop(stop);

			return RedirectToAction("Index", "Schedule", new { @name = truckurlslug });
		}

		[HttpGet]
		[Route("schedule/edit/{id}")]
		public ActionResult Edit(int id)
		{

			var stop = schedService.GetStop(id);
			if(stop == null)
			{
				return HttpNotFound();
			}

			var vm = mapper.Map<ScheduledStopEditViewModel>(stop);
			vm.SelectedFrom = stop.From.ToString("h:mm tt");
			vm.SelectedTo = stop.To.ToString("h:mm tt");
			var timeSlots = CreateTimeSlots();
			vm.From = timeSlots;
			vm.To = timeSlots;
			var locations = schedService.GetLocations();
			vm.Locations = mapper.Map<IEnumerable<SelectListItem>>(locations);

			return View(vm);
		}

		[HttpPost]
		[Route("schedule/edit/{id}")]
		public ActionResult Edit(ScheduledStopEditViewModel vm, int id)
		{
			var stop = schedService.GetStop(id);
			
			DateTime from;
			DateTime to;
			DateTime.TryParseExact(vm.SelectedFrom, @"h\:mm tt", CultureInfo.CurrentCulture, DateTimeStyles.None, out from);
			DateTime.TryParseExact(vm.SelectedTo, @"h\:mm tt", CultureInfo.CurrentCulture, DateTimeStyles.None, out to);
			if (!vm.Recurring && stop.Recurring)
			{
				
				var newStop = new ScheduledStop();
				newStop = mapper.Map<ScheduledStop>(vm);
				newStop.FoodTruckId = stop.FoodTruckId;
				newStop.From = new DateTime(vm.Day.Value.Year, vm.Day.Value.Month, vm.Day.Value.Day, from.Hour, from.Minute, from.Second);
				newStop.To = new DateTime(vm.Day.Value.Year, vm.Day.Value.Month, vm.Day.Value.Day, to.Hour, to.Minute, to.Second);
				newStop.LastModifiedOn = DateTime.Now;
				newStop.Recurring = false;
				//create the 'new' stop
				schedService.CreateStop(newStop);
				//update the existing recurring stop with the exception
				schedService.CreateStopException(newStop.From, stop.Id);
			}
			else
			{
				stop.LocationId = vm.LocationId;
				stop.Recurring = vm.Recurring;
				stop.RecurringStart = vm.RecurringStart;
				stop.HasRecurringEnd = vm.HasRecurringEnd;
				stop.RecurringEnd = vm.RecurringEnd;
				stop.From = new DateTime(vm.Day.Value.Year, vm.Day.Value.Month, vm.Day.Value.Day, from.Hour, from.Minute, from.Second);
				stop.To = new DateTime(vm.Day.Value.Year, vm.Day.Value.Month, vm.Day.Value.Day, to.Hour, to.Minute, to.Second);
				stop.LastModifiedOn = DateTime.Now;
				schedService.EditStop(stop);
			}

			return RedirectToAction("Index", "Schedule", new { @name = stop.FoodTruck.UrlSlug });
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
			//var days = schedService.GetTruckSchedule(urlslug);
			var days = schedService.GetTruckSchedule(urlslug, from, to);

			List<DailyScheduleViewModel> daysInSchedule = new List<DailyScheduleViewModel>();
			foreach (ScheduleDay d in days)
			{
				daysInSchedule.Add(new DailyScheduleViewModel()
				{
					Day = d.Day,
					Stops = mapper.Map<IEnumerable<ScheduledStopDetailsViewModel>>(d.Stops)
				});
			}

			var vm = new WeeklyScheduleViewModel();
			vm.Days = daysInSchedule;

			return PartialView(vm);
		}
	}
}