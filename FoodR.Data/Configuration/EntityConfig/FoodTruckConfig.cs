using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using FoodR.Data.Models;

namespace FoodR.Data.Configuration.EntityConfig
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