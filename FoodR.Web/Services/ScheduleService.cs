using FoodR.Data;
using FoodR.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodR.Web.Services
{
	public class ScheduleService : IScheduleService
	{
		private readonly IRepository repository;
		private readonly ITruckService truckService;

		public ScheduleService(IRepository repo, ITruckService truckService)
		{
			this.repository = repo;
			this.truckService = truckService;
		}

		public IEnumerable<ScheduleDay> GetTruckSchedule(string urlslug, DateTime? fromDay = null, DateTime? toDay = null)
		{
			FoodTruck truck = truckService.GetTruckByUrl(urlslug);
			List<ScheduledStop> scheduledStops = new List<ScheduledStop>();

			if (fromDay.HasValue && toDay.HasValue)
			{
				scheduledStops.AddRange(repository.Where<ScheduledStop>(
					e => e.FoodTruckId == truck.Id
						&& e.From >= fromDay
						&& e.To <= toDay));

				var recurringStops = repository.Where<RecurringStop>(
					e => e.FoodTruckId == truck.Id
					&& e.Starting.DayOfWeek == fromDay.Value.DayOfWeek
					&& e.Starting.TimeOfDay >= fromDay.Value.TimeOfDay
					&& e.Ending.TimeOfDay <= toDay.Value.TimeOfDay
					&& !(e.Exceptions.Any(ee => ee.Day.Date == fromDay.Value.Date))
					);



				foreach (var stop in recurringStops)
				{
					scheduledStops.AddRange(CreateRecurringInstances(stop, fromDay.Value, toDay.Value));
				}
			}
			else
			{
				scheduledStops.AddRange(repository.Where<ScheduledStop>(e => e.FoodTruckId == truck.Id));
			}

			var groups = scheduledStops.GroupBy(e => e.From.Date);
			var scheduleDays = new List<ScheduleDay>();
			foreach (var g in groups)
			{
				scheduleDays.Add(new ScheduleDay()
				{
					Day = g.Key,
					Entries = g
				});
			}

			return scheduleDays.ToArray();
		}

		//generate instances of a recurring stop for the given time span
		private IEnumerable<ScheduledStop> CreateRecurringInstances(RecurringStop stop, DateTime from, DateTime to)
		{
			var instances = new List<ScheduledStop>();

			DateTime possibleStopTime = new DateTime(from.Year, from.Month, from.Day, stop.Starting.Hour, stop.Starting.Minute, stop.Starting.Second);
			if (possibleStopTime < stop.Starting)
				possibleStopTime = stop.Starting;

			while (possibleStopTime >= from && possibleStopTime <= to)
			{
				while (possibleStopTime.DayOfWeek != stop.Starting.DayOfWeek)
				{
					possibleStopTime = possibleStopTime.AddDays(1);
				}

				if (!stop.Exceptions.Any(e => e.Day.Date == possibleStopTime.Date))
				{
					instances.Add(new ScheduledStop()
					{
						Active = true,
						From = possibleStopTime,
						To = CreateToDate(possibleStopTime, stop.Ending.TimeOfDay)
					});
				}

				possibleStopTime = possibleStopTime.AddDays(1);
			}

			return instances;
		}

		private DateTime CreateToDate(DateTime from, TimeSpan to)
		{
			DateTime toDate = new DateTime(from.Year, from.Month, from.Day, to.Hours, to.Minutes, to.Seconds);
			if (from.TimeOfDay > to)
				toDate = toDate.AddDays(1);

			return toDate;
		}

		public ScheduledStop GetScheduledStopById(int id)
		{
			return repository.Find<ScheduledStop>(id);
		}

		public ServiceCallResult EditScheduledStop(ScheduledStop entry)
		{
			ServiceCallResult result = new ServiceCallResult() { Success = false };

			repository.SaveChanges();
			result.Success = true;

			return result;
		}

		public ServiceCallResult CreateScheduledStop(ScheduledStop entry)
		{
			ServiceCallResult result = new ServiceCallResult() { Success = false };

			try
			{
				repository.Add<ScheduledStop>(entry);
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
	}

	public interface IScheduleService
	{
		IEnumerable<ScheduleDay> GetTruckSchedule(string urlslug, DateTime? fromDay = null, DateTime? toDay = null);
		ScheduledStop GetScheduledStopById(int id);
		ServiceCallResult EditScheduledStop(ScheduledStop entry);
		ServiceCallResult CreateScheduledStop(ScheduledStop entry);
	}

	public class ScheduleDay
	{
		public DateTime Day { get; set; }
		public IEnumerable<ScheduledStop> Entries { get; set; }
	}
}