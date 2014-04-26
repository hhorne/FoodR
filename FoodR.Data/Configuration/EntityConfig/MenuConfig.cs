using FoodR.Web.Data.Models;

namespace FoodR.Data.Configuration.EntityConfig
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