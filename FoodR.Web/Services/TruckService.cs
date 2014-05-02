﻿using FoodR.Data;
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

			var days = new Dictionary<DateTime, List<ScheduleEntry>>();
			foreach (ScheduleEntry e in entries)
			{
				DateTime entryDay = e.From.Date;
				
				if (!days.ContainsKey(entryDay))
				{

					days.Add(entryDay, new List<ScheduleEntry>() { e });
				}
				else
					days[entryDay].Add(e);
			}

			var scheduleDays = new List<ScheduleDay>();
			foreach(var d in days)
			{
				scheduleDays.Add(new ScheduleDay()
				{
					Day = d.Key,
					Entries = d.Value
				});
			}
			return scheduleDays.ToArray();
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
		IEnumerable<ScheduleDay> GetTruckSchedule(string urlslug, DateTime? fromDay = null, DateTime? toDay = null);
	}

	public class TruckResult
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