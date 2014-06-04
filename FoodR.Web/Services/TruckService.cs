using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FoodR.Data;
using FoodR.Data.Models;

namespace FoodR.Web.Services
{
	public interface ITruckService
	{
		ServiceCallResult CreateTruck(FoodTruck truck);

		ServiceCallResult EditTruck(FoodTruck truck);

		IEnumerable<Category> GetCategories();

		FoodTruck GetTruckById(int id);

		FoodTruck GetTruckByUrl(string name);

		IEnumerable<FoodTruck> GetTrucks(DateTime? day = null);

		IEnumerable<FoodTruck> GetTrucksByCategory(int id);
	}

	public class ServiceCallResult
	{
		public IEnumerable<string> Errors { get; set; }

		public bool Success { get; set; }
	}

	public class TruckService : ITruckService
	{
		private readonly IRepository repository;

		public TruckService(IRepository repo)
		{
			repository = repo;
		}

		public ServiceCallResult CreateTruck(FoodTruck truck)
		{
			ServiceCallResult result = new ServiceCallResult() { Success = false };

			try
			{
				truck.UrlSlug = GenerateUrlSlug(truck.Name);

				AttachCategories(truck);

				repository.Add<FoodTruck>(truck);
				repository.SaveChanges();
				result.Success = true;
			}
			catch (Exception ex)
			{
				string[] errors = new[] { ex.Message };
				result.Errors = errors;
				result.Success = false;
			}
			return result;
		}

		private void AttachCategories(FoodTruck truck)
		{
			IEnumerable<Category> categories = truck.Categories;
			truck.Categories = new List<Category>();

			foreach (var category in categories)
			{
				repository.Attach<Category>(category);
				truck.Categories.Add(category);
			}
		}

		public ServiceCallResult EditTruck(FoodTruck truck)
		{
			ServiceCallResult result = new ServiceCallResult() { Success = false };

			try
			{
				AttachCategories(truck);
				repository.SaveChanges();
				result.Success = true;
			}
			catch (Exception ex)
			{
				string[] errors = new[] { ex.Message };
				result.Errors = errors;
				result.Success = false;
			}
			return result;
		}

		public IEnumerable<Category> GetCategories()
		{
			IEnumerable<Category> categories = null;
			categories = repository.GetAll<Category>();
			return categories;
		}

		public FoodTruck GetTruckById(int id)
		{
			return repository.Where<FoodTruck>(t => t.Id == id).FirstOrDefault();
		}

		public FoodTruck GetTruckByUrl(string name)
		{
			return repository.Where<FoodTruck>(t => t.UrlSlug == name).FirstOrDefault();
		}

		public IEnumerable<FoodTruck> GetTrucks(DateTime? day = null)
		{
			IEnumerable<FoodTruck> trucks = null;
			if (day.HasValue)
			{
				DateTime thisDay = day.Value.Date;
				DateTime nextDay = thisDay + TimeSpan.FromDays(1);
				trucks = repository.Where<FoodTruck>(t => t.ScheduledStops.Any(e => e.From >= thisDay && e.From < nextDay));
			}
			else
				trucks = repository.GetAll<FoodTruck>();

			IEnumerable<Category> cat = null;
			cat = repository.GetAll<Category>();
			IEnumerable<Category> cats = cat;
			return trucks;
		}

		public IEnumerable<FoodTruck> GetTrucksByCategory(int id)
		{
			IEnumerable<FoodTruck> trucks = null;

			if (id == default(int))
			{
				return trucks;
			}

			trucks = repository.Where<FoodTruck>(t => t.Categories.Any(e => e.Id == id), "Categories");

			return trucks;
		}

		private string GenerateUrlSlug(string name)
		{
			const char REPLACECHAR = '-';
			string urlslug = Regex.Replace(name, "[^A-Za-z0-9-]+", REPLACECHAR.ToString()).Trim(REPLACECHAR);
			string orignal = urlslug;
			int cnt = 1;
			while (repository.GetAll<FoodTruck>().FirstOrDefault(t => t.UrlSlug == urlslug) != null)
			{
				cnt++;
				urlslug = orignal + cnt.ToString();

				if (cnt > 100000)
					throw new Exception("How in the hell did this happen?!");
			}

			return urlslug;
		}
	}
}