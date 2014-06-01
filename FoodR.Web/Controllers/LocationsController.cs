using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
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

		[Route("locations/{name?}", Name = "Locations")]
	    public ActionResult Index(string name)
	    {
			if (name.IsNullOrEmpty())
			{
				return View("Areas", GetAreaViewModels());
			}

			var vm = GetLocationViewModel(name);
			if (vm == null)
			{
				return HttpNotFound();
			}

			return View("Location", vm);
	    }

		[Route("locations/areas")]
	    public ActionResult Areas()
		{
			var vm = GetAreaViewModels();
			return View(vm);
	    }
		
		[HttpGet]
		[Route("locations/create")]
	    public ActionResult Create()
		{
			var vm = new LocationEditViewModel
			{
				Areas = mapper.Map<IEnumerable<SelectListItem>>(repository.GetAll<Area>())
			};

		    return View(vm);
	    }

	    [HttpPost]
		[Route("locations/create")]
		public ActionResult Create(LocationEditViewModel viewModel)
	    {
		    if (ModelState.IsValid)
		    {
			    var location = new Location
			    {
				    Name = viewModel.Name,
					StreetAddress = viewModel.StreetAddress,
					StreetAddress2 = viewModel.StreetAddress2,
					ZipCode = viewModel.ZipCode,
				    AreaId = viewModel.AreaId,
					UrlSlug = viewModel.Name.ConvertToUrlSlug()
			    };

				repository.Add(location);
			    repository.SaveChanges();
			    return RedirectToRoute("Locations");
		    }

		    return View(viewModel);
	    }

		[HttpGet]
		[Route("locations/edit/{id}")]
		public ActionResult Edit(int id)
		{
			var location = repository.Find<Location>(new object[] { id });
			var viewModel = mapper.Map<LocationEditViewModel>(location);
			viewModel.Areas = mapper.Map<IEnumerable<SelectListItem>>(repository.GetAll<Area>());
			return View(viewModel);
		}

	    [HttpPost]
	    [Route("locations/edit/{id}")]
	    public ActionResult Edit(int id, LocationEditViewModel viewModel)
	    {
		    if (ModelState.IsValid)
		    {
			    var location = repository.Find<Location>(id);
			    if (location == null)
			    {
				    return HttpNotFound();
			    }

				location = mapper.Map(viewModel, location);
				repository.SaveChanges();

			    return RedirectToRoute("Locations", new { name = location.UrlSlug });
		    }

		    return View(viewModel);
	    }

		[HttpGet]
		[Route("locations/map")]
		public ActionResult Map()
		{
			MapViewModel m = new MapViewModel();

			using (FoodRContext db = new FoodRContext())
			{
				//Find all the trucks out right now.
				//TODO: Prolly want to expand this to check for trucks that will be out with in an hour from now, maybe.
				//TESTING: this date should be changed to now, but for testing im leaving it like this
				var myDate = new DateTime(2014, 4, 23, 11, 15, 0);
				m.Trucks = db.FoodTrucks.Where(t => t.ScheduledStops.Any(e => e.From < myDate && e.To > myDate)).ToArray();
			}

			return View(m);
		}

	    private IEnumerable<AreaDetailViewModel> GetAreaViewModels()
		{
			var areas = repository.GetAll<Area>().ToArray();
			var areasVm = mapper.Map<IEnumerable<AreaDetailViewModel>>(areas);
		    return areasVm;
		}

	    private LocationDetailViewModel GetLocationViewModel(string name)
		{
			var location = repository.Where<Location>(
				l => l.UrlSlug.Equals(name, StringComparison.OrdinalIgnoreCase)
			).SingleOrDefault();

		    if (location == null)
				return null;

			var vm = mapper.Map<LocationDetailViewModel>(location);
			return vm;
		}
    }
}