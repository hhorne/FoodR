using System;
using System.Collections.Generic;
using System.Linq;
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
				Areas = mapper.Map<IEnumerable<SelectListItem>>(GetAreaViewModels())
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
				    AreaId = viewModel.AreaId,
					UrlSlug = viewModel.Name.ConvertToUrlSlug()
			    };

				repository.Add(location);
			    repository.SaveChanges();
			    return RedirectToRoute("Locations");
		    }

		    return View(viewModel);
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