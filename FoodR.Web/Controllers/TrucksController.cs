using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using FoodR.Data;
using FoodR.Data.Models;
using FoodR.Web.ViewModels;
using FoodR.Web.Services;
using System;

namespace FoodR.Web.Controllers
{
    public class TrucksController : FoodRController
    {
		private readonly ITruckService service;
	    private readonly IMappingEngine mapper;

	    public TrucksController(ITruckService service, IMappingEngine mapper)
	    {
		    this.service = service;
		    this.mapper = mapper;
	    }

	    public ActionResult Index()
	    {
		    var trucks = service.GetTrucks();
			var details = mapper.Map<IEnumerable<TruckListDetailViewModel>>(trucks);
            return View(details);
        }

		[Route("trucks/details/{name}")]
		public ActionResult Details(string name)
		{
			var truck = service.GetTruckByUrl(name);
			if (truck == null)
			{
				return HttpNotFound();
			}

			var viewModel = mapper.Map<TruckDetailsViewModel>(truck);
			
			return View(viewModel);
		}
		
		[HttpGet]
		[Route("trucks/create")]
		public ActionResult Create()
		{
			return View(new TruckEditViewModel());
		}

		[HttpPost]
		[Route("trucks/create")]
		public ActionResult Create(TruckEditViewModel vm)
		{
			FoodTruck newTruck = null;
			if (ModelState.IsValid)
			{
				try
				{
					newTruck = mapper.Map<FoodTruck>(vm);
					var result = service.CreateTruck(newTruck);

					vm.PageState = result.Success ? TruckDetailsPageState.SaveSuccessfully : TruckDetailsPageState.SaveFailed;
					vm.EditErrors = result.Errors;
				}
				catch (Exception ex)
				{
					vm.PageState = TruckDetailsPageState.SaveFailed;
					var errors = new string[] { ex.Message };
					vm.EditErrors = errors;
				}
			}
			else
			{
				vm.PageState = TruckDetailsPageState.SaveFailed;
			}
			return RedirectToAction("Index", "Admin");
		}

		[HttpGet]
		[Route("trucks/edit/{slug}")]
		public ActionResult Edit(string slug)
		{
			var truck = service.GetTruckByUrl(slug);
			if (truck == null)
			{
				return HttpNotFound();
			}

			ViewBag.UrgSlug = slug;

			var vm = mapper.Map<TruckEditViewModel>(truck);
			return View(vm);
		}

		[HttpPost]
		[Route("trucks/edit/{slug}")]
		public ActionResult Edit(TruckEditViewModel model, string slug)
		{
			if (ModelState.IsValid)
			{
				try
				{
					FoodTruck truck = service.GetTruckByUrl(slug);
					truck = mapper.Map(model, truck);
					var result = service.EditTruck(truck);

					model.PageState = result.Success ? TruckDetailsPageState.SaveSuccessfully : TruckDetailsPageState.SaveFailed;
					model.EditErrors = result.Errors;
				}
				catch (Exception ex)
				{
					model.PageState = TruckDetailsPageState.SaveFailed;
					var errors = new string [] { ex.Message };
					model.EditErrors = errors;
				}
			}
			else
			{
				model.PageState = TruckDetailsPageState.SaveFailed;
			}
			return View(model);
		}

		[HttpGet]
		public ActionResult WeeklySchedule(string urlslug)
		{
			DateTime from = DateTime.Now.Date.AddDays(-1);
			DateTime to = from.AddDays(7);
			var vm = new WeeklyScheduleViewModel();
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

			vm.Days = daysInSchedule;

			return PartialView(vm);
		}
	}
}