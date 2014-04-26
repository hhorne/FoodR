using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using FoodR.Data;
using FoodR.Data.Models;
using FoodR.Web.ViewModels;

namespace FoodR.Web.Controllers
{
    public class TrucksController : FoodRController
    {
	    private readonly IRepository repository;
	    private readonly IMappingEngine mapper;

	    public TrucksController(IRepository repository, IMappingEngine mapper)
	    {
		    this.repository = repository;
		    this.mapper = mapper;
	    }

	    public ActionResult Index()
	    {
		    var trucks = repository.GetAll<FoodTruck>();
			var details = mapper.Map<IEnumerable<TruckDetailsViewModel>>(trucks);
            return View(details);
        }
	}
}