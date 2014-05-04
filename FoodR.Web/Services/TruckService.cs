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

		public ServiceCallResult CreateTruck(FoodTruck truck)
		{
			ServiceCallResult result = new ServiceCallResult() { Success = false };

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

		public ServiceCallResult EditTruck(FoodTruck truck)
		{
			ServiceCallResult result = new ServiceCallResult() { Success = false };

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
				trucks = repository.Where<FoodTruck>(t => t.ScheduleEntries.Any(e => e.From >= thisDay && e.From < nextDay));
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

		public IEnumerable<ScheduleDay> GetTruckSchedule(string urlslug, DateTime? fromDay = null, DateTime? toDay = null)
		{
			FoodTruck truck = GetTruckByUrl(urlslug);
			IEnumerable<ScheduleEntry> entries;
			
			if (fromDay.HasValue && toDay.HasValue)
				entries = repository.Where<ScheduleEntry>(e => e.FoodTruckId == truck.Id && e.From >= fromDay && e.To <= toDay);
			else
				entries = repository.Where<ScheduleEntry>(e => e.FoodTruckId == truck.Id);

			var groups = entries.GroupBy(e => e.From.Date);
			var scheduleDays = new List<ScheduleDay>();
			foreach(var g in groups)
			{
				scheduleDays.Add(new ScheduleDay()
				{
					Day = g.Key,
					Entries = g
				});
			}

			return scheduleDays.ToArray();
		}

		public ScheduleEntry GetScheduleEntryById(int id)
		{
			return repository.Find<ScheduleEntry>(id);
		}

		public ServiceCallResult EditScheduleEntry(ScheduleEntry entry)
		{
			ServiceCallResult result = new ServiceCallResult() { Success = false };

			repository.SaveChanges();
			result.Success = true;

			return result;
		}

		public ServiceCallResult CreateScheduleEntry(ScheduleEntry entry)
		{
			ServiceCallResult result = new ServiceCallResult() { Success = false };

			try
			{
				repository.Add<ScheduleEntry>(entry);
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
		ServiceCallResult CreateTruck(FoodTruck truck);
		ServiceCallResult EditTruck(FoodTruck truck);
		IEnumerable<FoodTruck> GetTrucks(DateTime? day = null);
		FoodTruck GetTruckByUrl(string name);
		FoodTruck GetTruckById(int id);
		IEnumerable<ScheduleDay> GetTruckSchedule(string urlslug, DateTime? fromDay = null, DateTime? toDay = null);
		ScheduleEntry GetScheduleEntryById(int id);
		ServiceCallResult EditScheduleEntry(ScheduleEntry entry);
		ServiceCallResult CreateScheduleEntry(ScheduleEntry entry);
	}

	public class ServiceCallResult
	{
		public bool Success { get; set; }
		public IEnumerable<string> Errors { get; set; }
	}

	public class ScheduleDay
	{
		public DateTime Day { get; set; }
		public IEnumerable<ScheduleEntry> Entries { get; set; }
	}
}