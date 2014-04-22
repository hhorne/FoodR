using FoodR.Web.Data.Models;
namespace FoodR.Web.Data.EntityTypeConfiguration
{
	public class ScheduleEntryConfig : EntityConfig<ScheduleEntry>
	{
		public ScheduleEntryConfig()
		{
			Property(s => s.Comments).
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