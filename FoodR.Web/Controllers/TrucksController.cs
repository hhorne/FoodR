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
			var details = mapper.Map<IEnumerable<TruckDetailsViewModel>>(trucks);
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

		[Route("trucks/edit/{truckName}")]
		public ActionResult Edit(string truckName)
		{
			var truck = service.GetTruckByUrl(truckName);
			if (truck == null)
			{
				return HttpNotFound();
			}

			ViewBag.TruckName = truckName;

			var viewModel = mapper.Map<TruckEditViewModel>(truck);
			return View(viewModel);
		}

		[HttpPost]
		[Route("trucks/edit/{truckName}")]
		public ActionResult Edit(TruckEditViewModel model, string truckName)
		{
			if (ModelState.IsValid)
			{
				try
				{
					FoodTruck truck = service.GetTruckByUrl(truckName);
					truck = mapper.Map(model, truck);
					var result = service.SaveTruck(truck);

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
	}
}