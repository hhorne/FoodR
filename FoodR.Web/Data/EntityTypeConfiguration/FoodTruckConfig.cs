using System.Data.Entity.ModelConfiguration;
using FoodR.Web.Data.Models;

namespace FoodR.Web.Data.EntityTypeConfiguration
{
	public class FoodTruckConfig : EntityConfig<FoodTruck>, IEntityConfig
	{
		public FoodTruckConfig()
		{
			Property(t => t.Name).HasMaxLength(50);
			Property(t => t.Description).HasMaxLength(500);
		}
	}
}