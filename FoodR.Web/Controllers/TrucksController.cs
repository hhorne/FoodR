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

		[HttpGet]
		public ActionResult Index()
		{
			var trucks = service.GetTrucks();
			var details = mapper.Map<IEnumerable<TruckListDetailViewModel>>(trucks);
			ViewBag.Categories = GetCategories();
			return View(details);
		}

		[HttpPost]
		public ActionResult Index(FormCollection value)
		{
			if (value == null)
			{
				return View(new List<TruckListDetailViewModel>());
			}

			int id = 0;
			int.TryParse(value["category"], out id);

			if(id == 0)
			{
				return RedirectToAction("Index");
			}

			return Redirect(string.Format("/trucks/category/{0}",id));
		}

		[HttpGet]
		[Route("trucks/category/{id}")]
		public ActionResult Index(int id)
		{
			var trucks = service.GetTrucksByCategory(id);
			var details = mapper.Map<IEnumerable<TruckListDetailViewModel>>(trucks);
			ViewBag.Categories = GetCategories();
			ViewBag.CategoryId = id.ToString();
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

		[Authorize]
		[HttpGet]
		[Route("trucks/create")]
		public ActionResult Create()
		{
			TruckEditViewModel model = new TruckEditViewModel();
			model.Categories = GetCategories();
			return View(model);
		}

		[Authorize]
		[HttpPost]
		[Route("trucks/create")]
		public ActionResult Create(TruckEditViewModel vm)
		{
			if (!ModelState.IsValid)
			{
				vm.PageState = TruckDetailsPageState.SaveFailed;

				var errors = GetErrors(ModelState);

				return View(vm);
			}

			try
			{
				FoodTruck newTruck = null;
				newTruck = mapper.Map<FoodTruck>(vm);
				newTruck.Categories = GetSelectedCategory(vm);
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

			return RedirectToAction("Index", "Admin");
		}

		[Authorize]
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

			Mapper
				.CreateMap<FoodTruck, TruckEditViewModel>()
					.ForMember(m => m.Categories, opt => opt.Ignore());

			var vm = Mapper.Map<TruckEditViewModel>(truck);

			vm.Categories = GetCategories();
			vm.CategoryId = GetCategoryId(truck.Categories);

			return View(vm);
		}

		[Authorize]
		[HttpPost]
		[Route("trucks/edit/{slug}")]
		public ActionResult Edit(TruckEditViewModel vm, string slug)
		{
			if (!ModelState.IsValid)
			{
				vm.PageState = TruckDetailsPageState.SaveFailed;

				var errors = GetErrors(ModelState);

				return View(vm);
			}

			try
			{
				FoodTruck truck = service.GetTruckByUrl(slug);
				truck = mapper.Map(vm, truck);
				truck.Categories = GetSelectedCategory(vm);
				var result = service.EditTruck(truck);

				vm.PageState = result.Success ? TruckDetailsPageState.SaveSuccessfully : TruckDetailsPageState.SaveFailed;
				vm.EditErrors = result.Errors;
			}
			catch (Exception ex)
			{
				vm.PageState = TruckDetailsPageState.SaveFailed;
				var errors = new string[] { ex.Message };
				vm.EditErrors = errors;
			}

			return RedirectToAction("Index", "Admin");
		}

		private IEnumerable<dynamic> GetErrors(ModelStateDictionary state)
		{
			var errors = ModelState
					.Where(x => x.Value.Errors.Count > 0)
					.Select(x => new { x.Key, x.Value.Errors })
					.ToArray();

			return errors;
		}

		private IEnumerable<System.Web.WebPages.Html.SelectListItem> GetCategories()
		{
			List<System.Web.WebPages.Html.SelectListItem> items = new List<System.Web.WebPages.Html.SelectListItem>();

			var selectOne = new System.Web.WebPages.Html.SelectListItem { Text = "Select One", Value = "0" };
			items.Add(selectOne);

			IEnumerable<Category> categories = service.GetCategories();

			foreach (var category in categories)
			{
				System.Web.WebPages.Html.SelectListItem item = new System.Web.WebPages.Html.SelectListItem();
				item.Text = category.Name;
				item.Value = category.Id.ToString();
				items.Add(item);
			}

			return items;
		}

		private ICollection<Category> GetSelectedCategory(TruckEditViewModel vm)
		{
			ICollection<Category> categories = new List<Category>();

			Category cat = new Category();
			cat.Id = vm.CategoryId;
			categories.Add(cat);

			return categories;
		}

		private int GetCategoryId(ICollection<Category> categories)
		{
			if (categories == null)
			{
				return 0;
			}

			Category category = categories.FirstOrDefault();

			if (category == null)
			{
				return 0;
			}

			return category.Id;
		}
	}
}