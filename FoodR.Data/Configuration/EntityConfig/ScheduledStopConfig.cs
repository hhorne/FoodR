using FoodR.Data.Models;

namespace FoodR.Data.Configuration.EntityConfig
{
	public class ScheduledStopConfig : EntityConfig<ScheduledStop>
	{
		public ScheduledStopConfig()
		{
			Property(s => s.ModifiedComments).
				HasMaxLength(500);

			HasRequired(s => s.FoodTruck).
				WithMany().
				HasForeignKey(s => s.FoodTruckId);

			HasRequired(s => s.Location).
				WithMany().
				HasForeignKey(s => s.LocationId).
				WillCascadeOnDelete(false);
			
			//HasRequired(s => s.RecurringStop).
			//	WithMany().
			//	WillCascadeOnDelete(false);

			//HasOptional(s => s.RecurringStop).
			//	WithMany().
			//	HasForeignKey(s => s.RecurringStopId).
			//	WillCascadeOnDelete(false);
		}
	}
}