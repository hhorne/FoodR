using FoodR.Data.Models;

namespace FoodR.Data.Configuration.EntityConfig
{
	public class RecurringStopConfig : EntityConfig<RecurringStop>
	{
		public RecurringStopConfig()
		{
			HasRequired(s => s.FoodTruck).
				WithMany().
				HasForeignKey(s => s.FoodTruckId);

			HasRequired(s => s.Location).
				WithMany().
				HasForeignKey(s => s.LocationId).
				WillCascadeOnDelete(false);
		}
	}
}
