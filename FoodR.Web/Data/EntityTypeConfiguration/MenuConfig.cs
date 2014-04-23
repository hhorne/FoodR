using FoodR.Web.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodR.Web.Data.EntityTypeConfiguration
{
	public class MenuConfig : EntityConfig<Menu>
	{
		public MenuConfig()
		{
			Property(t => t.Name).HasMaxLength(50);
			HasRequired(s => s.FoodTruck).
				WithMany().
				HasForeignKey(s => s.FoodTruckId);
		}
	}

	public class MenuItemConfig : EntityConfig<MenuItem>
	{
		public MenuItemConfig()
		{
			HasRequired(s => s.Menu).
				WithMany().
				HasForeignKey(s => s.MenuId);

			Property(t => t.Name).HasMaxLength(50);
			Property(t => t.Description).HasMaxLength(50);
		}
	}
}