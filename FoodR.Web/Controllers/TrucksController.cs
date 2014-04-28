using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using FoodR.Data;
using FoodR.Data.Models;
using FoodR.Web.ViewModels;
using FoodR.Web.Services;

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
			var details = mapper.Map<IEnumerable<TruckDetailsViewModel>>(trucks);
            return View(details);
        }

		public ActionResult Details(string truckName)
		{
			var truck = service.GetTruck(truckName);
			var details = mapper.Map<TruckDetailsViewModel>(truck);
			return View(details);
		}

		public ActionResult Edit(string truckName)
		{
			var truck = service.GetTruck(truckName);
			var details = mapper.Map<TruckDetailsViewModel>(truck);
			details.PageState = TruckDetailsPageState.Loaded;

			return View(details);
		}

		[HttpPost]
		public ActionResult Edit(TruckDetailsViewModel model, string truckName)
		{
			var truck = service.GetTruck(truckName);

			truck.Name = model.Name;
			truck.Description = model.Description;
			truck.Website = model.Website;

			var result = service.SaveTruck(truck);
			var details = mapper.Map<TruckDetailsViewModel>(truck);
			details.PageState = TruckDetailsPageState.SaveSuccessfully;
			if (!result.Success)
			{
				details.EditErrors = result.Errors;
				details.PageState = TruckDetailsPageState.SaveFailed;
			}

			return View(details);
		}
	}
}