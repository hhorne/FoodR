using FoodR.Web.Data;
using FoodR.Web.Data.Models;
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

			return result;
		}

		public IEnumerable<FoodTruck> GetAllTrucks(DateTime? day = null)
		{
			var trucks = repository.GetAll<FoodTruck>();
			if(day.HasValue)
			{
				DateTime thisDay = day.Value.Date;
				DateTime nextDay = thisDay + TimeSpan.FromDays(1);
				trucks = trucks.Where(t => t.Events.Any(e => e.From >= thisDay && e.From < nextDay));
			}

			return trucks;
		}
	}

	public interface ITruckService
	{
		TruckResult SaveTruck(FoodTruck truck);
		IEnumerable<FoodTruck> GetAllTrucks(DateTime? day = null);
		
	}

	public class TruckResult
	{
		public bool Success { get; set; }
		public IEnumerable<string> Errors { get; set; }
	}
}