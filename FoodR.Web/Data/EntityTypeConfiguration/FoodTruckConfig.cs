using System.Data.Entity.ModelConfiguration;
using FoodR.Web.Data.Models;

namespace FoodR.Web.Data.EntityTypeConfiguration
{
	public class FoodTruckConfig : EntityConfig<FoodTruck>
	{
		public FoodTruckConfig()
		{
			Property(t => t.Name).HasMaxLength(50);
			Property(t => t.Description).HasMaxLength(500);
			Property(t => t.Phone).HasMaxLength(20);
			Property(t => t.Phone2).HasMaxLength(20);
			Property(t => t.Email).HasMaxLength(100);
			Property(t => t.Email2).HasMaxLength(100);
			Property(t => t.Website).HasMaxLength(100);
			Property(t => t.Twitter).HasMaxLength(100);
			Property(t => t.Facebook).HasMaxLength(100);
		}
	}
}