using FoodR.Data;
using FoodR.Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

		public IEnumerable<FoodTruck> GetSchedulesByDay(DateTime day)
		{
			try
			{
				var date = day.Date;

				var trucks = repository.GetAll<FoodTruck>().ToList();
				trucks = trucks.Select(t => new FoodTruck
				{
					Id = t.Id,
					Name = t.Name,
					Description = t.Description,
					UrlSlug = t.UrlSlug,
					Rating = t.Rating,
					Menus = t.Menus,
					ScheduledStops = t.ScheduledStops.Where(
						s => !s.Deleted &&
						(
							(!s.Recurring
							&& s.From.Year == date.Year
							&& s.From.Month == date.Month
							&& s.From.Day == date.Day)
						|| (s.Recurring
							&& s.RecurringStart.HasValue
							&& s.RecurringStart.Value.DayOfWeek == date.DayOfWeek
							&& s.RecurringStart.Value.Date <= date
							&& (!s.HasRecurringEnd || (s.HasRecurringEnd && s.RecurringEnd.Value.TimeOfDay <= date.TimeOfDay)))
						)
					).ToList()

				}).Where(t => t.ScheduledStops.Count() > 0).ToList();

				return trucks;
			}
			catch
			{
				throw;
			}
		}

		public IEnumerable<ScheduleDay> GetTruckSchedule(string urlslug, DateTime? fromDay = null, DateTime? toDay = null)
		{
			FoodTruck truck = truckService.GetTruckByUrl(urlslug);
			List<ScheduledStop> stops = new List<ScheduledStop>();

			if (fromDay.HasValue && toDay.HasValue)
			{
				var listOfDays = GetListOfDays(fromDay.Value, toDay.Value);

				var recurringStops = repository.Where<ScheduledStop>(
					e => e.FoodTruckId == truck.Id
					&& e.Recurring
					&& e.RecurringStart.HasValue
					&& listOfDays.Any(d => d == e.RecurringStart.Value.DayOfWeek)
					&& e.RecurringStart.Value.TimeOfDay >= fromDay.Value.TimeOfDay
					&& (!e.HasRecurringEnd || (e.HasRecurringEnd && e.RecurringEnd.Value.TimeOfDay <= toDay.Value.TimeOfDay))
					).ToList();

				recurringStops.ForEach(r => stops.AddRange(CreateRecurringInstances(r, fromDay.Value, toDay.Value)));

				var schedStops = repository.Where<ScheduledStop>(
					e => e.FoodTruckId == truck.Id
						&& !e.Recurring
						&& e.From >= fromDay
						&& e.To <= toDay).ToList();

				stops.AddRange(schedStops);
			}
			else
			{
				var schedStops = repository.Where<ScheduledStop>(e => e.FoodTruckId == truck.Id).ToList();
				stops.AddRange(schedStops);
			}

			var groups = stops.GroupBy(e => e.From.Date);
			var scheduleDays = new List<ScheduleDay>();
			foreach (var g in groups)
			{
				scheduleDays.Add(new ScheduleDay()
				{
					Day = g.Key,
					Stops = g
				});
			}

			return scheduleDays.ToArray();
		}

		public IEnumerable<Location> GetLocations()
		{
			return repository.GetAll<Location>();
		}

		public ServiceCallResult CreateStop(ScheduledStop stop)
		{
			ServiceCallResult result = new ServiceCallResult() { Success = false };

			try
			{
				repository.Add<ScheduledStop>(stop);
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

		public ServiceCallResult CreateStopException(DateTime day, int id)
		{
			ServiceCallResult result = new ServiceCallResult() { Success = false };
			RecurringException exception = new RecurringException()
			{
				Day = day.Date,
				ScheduledStopId = id
			};
			repository.Add<RecurringException>(exception);
			repository.SaveChanges();

			return result;
		}

		public ServiceCallResult EditStop(ScheduledStop stop)
		{
			ServiceCallResult result = new ServiceCallResult() { Success = false };

			repository.SaveChanges();

			return result;
		}

		public ServiceCallResult CancelStop(int id)
		{
			ServiceCallResult result = new ServiceCallResult() { Success = false };

			var stop = GetStop(id);

			stop.Canceled = true;

			result = EditStop(stop);

			return result;
		}

		public ServiceCallResult DeleteStop(int id)
		{
			ServiceCallResult result = new ServiceCallResult() { Success = false };

			var stop = GetStop(id);

			stop.Deleted = true;

			result = EditStop(stop);

			return result;
		}

		public ScheduledStop GetStop(int id)
		{
			return repository.Find<ScheduledStop>(id);
		}

		#region Privates
		private DayOfWeek[] GetListOfDays(DateTime from, DateTime to)
		{
			List<DayOfWeek> dow = new List<DayOfWeek>();
			DateTime day = from;
			while(day.Date <= to.Date)
			{
				dow.Add(day.DayOfWeek);
				day = day.AddDays(1);
			}

			return dow.ToArray();
		}

		//generate instances of a recurring stop for the given time span
		private IEnumerable<ScheduledStop> CreateRecurringInstances(ScheduledStop recurringStop, DateTime from, DateTime to)
		{
			var instances = new List<ScheduledStop>();
			DateTime possibleStopTime = CombineDateAndTime(from, recurringStop.From);
			if (possibleStopTime < recurringStop.RecurringStart.Value)
				possibleStopTime = CombineDateAndTime(recurringStop.RecurringStart.Value, recurringStop.From);

			while (possibleStopTime >= from && possibleStopTime <= to)
			{
				while (possibleStopTime.DayOfWeek != recurringStop.RecurringStart.Value.DayOfWeek)
				{
					possibleStopTime = possibleStopTime.AddDays(1);
				}

				if (possibleStopTime > to)
					break;

				DateTime possibleStopDate = possibleStopTime.Date;
				var exceptions = repository.GetAll<RecurringException>().Where(e => e.ScheduledStopId == recurringStop.Id);
				if (exceptions.Count() == 0 || !exceptions.Any(e => e.Day == possibleStopDate))
				{

					ScheduledStop newStop = new ScheduledStop()
					{
						Id = recurringStop.Id,
						From = possibleStopTime,
						To = CreateToDate(possibleStopTime, recurringStop.To.TimeOfDay),
						LocationId = recurringStop.LocationId,
						Location = recurringStop.Location,
						Recurring = true
					};
					instances.Add(newStop);
				}

				possibleStopTime = possibleStopTime.AddDays(1);
			}

			return instances;
		}

		private DateTime CombineDateAndTime(DateTime date, DateTime time)
		{
			return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
		}

		private DateTime CreateToDate(DateTime from, TimeSpan to)
		{
			DateTime toDate = new DateTime(from.Year, from.Month, from.Day, to.Hours, to.Minutes, to.Seconds);
			if (from.TimeOfDay > to)
				toDate = toDate.AddDays(1);

			return toDate;
		}
		#endregion
	}

	public interface IScheduleService
	{
		IEnumerable<FoodTruck> GetSchedulesByDay(DateTime day);
		IEnumerable<ScheduleDay> GetTruckSchedule(string urlslug, DateTime? fromDay = null, DateTime? toDay = null);
		IEnumerable<Location> GetLocations();
		ServiceCallResult CreateStop(ScheduledStop stop);
		ServiceCallResult CreateStopException(DateTime day, int id);
		ServiceCallResult EditStop(ScheduledStop stop);
		ServiceCallResult CancelStop(int id);
		ServiceCallResult DeleteStop(int id);
		ScheduledStop GetStop(int id);
	}

	public class ScheduleDay
	{
		public DateTime Day { get; set; }
		public IEnumerable<ScheduledStop> Stops { get; set; }
	}
}