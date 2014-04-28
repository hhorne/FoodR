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
		    var trucks = service.GetAllTrucks();
			var details = mapper.Map<IEnumerable<TruckDetailsViewModel>>(trucks);
            return View(details);
        }

		[Route("trucks/details/{name}")]
		public ActionResult Details(string name)
		{
			var truck = service.GetTruckByUrlName(name);
			if (truck == null)
			{
				return HttpNotFound();
			}

			var viewModel = mapper.Map<TruckDetailsViewModel>(truck);
			return View(viewModel);
		}

		public ActionResult Edit(string truckName)
		{

			return View();
		}
	}
}