using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using FoodR.Data;
using FoodR.Data.Models;
using FoodR.Web.ViewModels;

namespace FoodR.Web.Controllers
{
    public class LocationsController : Controller
    {
	    private readonly IRepository repository;
	    private readonly IMappingEngine mapper;

	    public LocationsController(IRepository repository, IMappingEngine mapper)
	    {
		    this.repository = repository;
		    this.mapper = mapper;
	    }

	    public ActionResult Index()
	    {
		    var areas = repository.GetAll<Area>().ToArray();
		    var viewModel = mapper.Map<IEnumerable<AreaDetailViewModel>>(areas);
            return View(areas);
        }
	}
}