using FoodR.Data.Models;

namespace FoodR.Data.Configuration.EntityConfig
{
	public class EventConfig : EntityConfig<ScheduleEntry>
	{
		public EventConfig()
		{
			Property(s => s.ModifiedComments).
				HasMaxLength(500);

			HasRequired(s => s.FoodTruck).
				WithMany().
				HasForeignKey(s => s.FoodTruckId);

			HasRequired(s => s.Location).
				WithMany().
				HasForeignKey(s => s.LocationId);
		}
	}
}