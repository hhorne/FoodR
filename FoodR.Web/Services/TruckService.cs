﻿using FoodR.Data;
using FoodR.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

		public TruckResult SaveTruck(FoodTruck truck)
		{
			TruckResult result = new TruckResult()
			{
				Success = false
			};

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

		public FoodTruck GetTruck(string name)
		{
			return repository.Where<FoodTruck>(t => t.UrlSlug == name).First();
		}
	}

	public interface ITruckService
	{
		TruckResult SaveTruck(FoodTruck truck);
		IEnumerable<FoodTruck> GetTrucks(DateTime? day = null);
		FoodTruck GetTruck(string name);
		
	}

	public class TruckResult
	{
		public bool Success { get; set; }
		public IEnumerable<string> Errors { get; set; }
	}
}