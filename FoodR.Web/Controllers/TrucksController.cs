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

		public ActionResult Details(string truckName)
		{

			return View();
		}

		public ActionResult Edit(string truckName)
		{

			return View();
		}
	}
}