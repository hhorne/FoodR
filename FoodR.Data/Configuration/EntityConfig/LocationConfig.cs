using FoodR.Data.Models;

namespace FoodR.Data.Configuration.EntityConfig
{
	public class LocationConfig : EntityConfig<Location>
	{
		public LocationConfig()
		{
			Property(l => l.Name).HasMaxLength(150);
			Property(l => l.UrlSlug).HasMaxLength(150);
			Property(a => a.StreetAddress).HasMaxLength(150);
			Property(a => a.StreetAddress2).HasMaxLength(150);
			Property(a => a.ZipCode).HasMaxLength(10);		
		}
	}
}