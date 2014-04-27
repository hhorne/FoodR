﻿using FoodR.Data.Models;

namespace FoodR.Data.Configuration.EntityConfig
{
	public class LocationConfig : EntityConfig<Location>
	{
		public LocationConfig()
		{
			Property(l => l.Name).HasMaxLength(150);
			Property(l => l.UrlSlug).HasMaxLength(150);			
		}
	}

	public class AreaConfig : EntityConfig<Area>
	{
		public AreaConfig()
		{
			Property(a => a.Name).HasMaxLength(150);
			Property(a => a.Description).HasMaxLength(500);
		}
	}
}