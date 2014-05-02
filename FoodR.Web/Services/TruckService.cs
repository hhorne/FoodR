using FoodR.Data;
using FoodR.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace FoodR.Web.Services
{
	public class TruckService : ITruckService
	{
		private readonly IRepository repository;

		public TruckService(IRepository repo)
		{
			repository = repo;
		}

		public TruckResult CreateTruck(FoodTruck truck)
		{
			TruckResult result = new TruckResult() { Success = false };

			try
			{
				truck.UrlSlug = GenerateUrlSlug(truck.Name);
				
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

		public TruckResult EditTruck(FoodTruck truck)
		{
			TruckResult result = new TruckResult() { Success = false };

			try
			{
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

		public IEnumerable<FoodTruck> GetTrucks(DateTime? day = null)
		{
			IEnumerable<FoodTruck> trucks = null;
			if(day.HasValue)
			{
				DateTime thisDay = day.Value.Date;
				DateTime nextDay = thisDay + TimeSpan.FromDays(1);
				trucks = repository.Where<FoodTruck>(t => t.Events.Any(e => e.From >= thisDay && e.From < nextDay));
			}
			else
				trucks = repository.GetAll<FoodTruck>();

			return trucks;
		}

		public FoodTruck GetTruckByUrl(string name)
		{
			return repository.Where<FoodTruck>(t => t.UrlSlug == name).FirstOrDefault();
		}

		public FoodTruck GetTruckById(int id)
		{
			return repository.Where<FoodTruck>(t => t.Id == id).FirstOrDefault();
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

	public interface ITruckService
	{
		TruckResult CreateTruck(FoodTruck truck);
		TruckResult EditTruck(FoodTruck truck);
		IEnumerable<FoodTruck> GetTrucks(DateTime? day = null);
		FoodTruck GetTruckByUrl(string name);
		FoodTruck GetTruckById(int id);
		
	}

	public class TruckResult
	{
		public bool Success { get; set; }
		public IEnumerable<string> Errors { get; set; }
	}
}