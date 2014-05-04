using AutoMapper;
using FoodR.Web.Services;
using FoodR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodR.Web.Controllers
{
	public class AdminController : FoodRController
    {
		private readonly ITruckService truckService;
		private readonly IMappingEngine mapper;

		public AdminController(ITruckService truckService, IMappingEngine mapper)
	    {
			this.truckService = truckService;
		    this.mapper = mapper;
	    }

		//[Authorize(Roles = "Admin")]
		[Authorize]
        public ActionResult Index()
        {
			var trucks = truckService.GetTrucks();
			var details = mapper.Map<IEnumerable<TruckDetailsViewModel>>(trucks);
			return View(details);
        }
	}
}