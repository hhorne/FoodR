using FoodR.Web.Data.Models;
namespace FoodR.Web.Data.EntityTypeConfiguration
{
	public class EventConfig : EntityConfig<Event>
	{
		public EventConfig()
		{
			Property(s => s.ModifiedComments).
				HasMaxLength(500);

			HasRequired(s => s.Truck).
				WithMany().
				HasForeignKey(s => s.FoodTruckId);

			HasRequired(s => s.Location).
				WithMany().
				HasForeignKey(s => s.LocationId);
		}
	}
}