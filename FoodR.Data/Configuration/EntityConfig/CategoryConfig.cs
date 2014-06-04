using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodR.Data.Models;

namespace FoodR.Data.Configuration.EntityConfig
{
	public class CategoryConfig : EntityConfig<Category>
	{
		public CategoryConfig()
		{
			Property(t => t.Name).HasMaxLength(50);

			HasMany(t => t.FoodTrucks)
				.WithMany(t => t.Categories)
				.Map(m =>
				{
					m.ToTable("FoodTruckCategory");
					m.MapLeftKey("FoodTruckId");
					m.MapRightKey("CategoryId");
				});
		}
	}
}
