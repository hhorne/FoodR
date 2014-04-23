using FoodR.Web.Data.Models;

namespace FoodR.Web.Data.EntityTypeConfiguration
{
	public class AddressConfig : EntityConfig<Address>
	{
		public AddressConfig()
		{
			Property(a => a.StreetAddress).HasMaxLength(150);
			Property(a => a.StreetAddress2).HasMaxLength(150);
			Property(a => a.City).HasMaxLength(150);
			Property(a => a.State).HasMaxLength(150);
			Property(a => a.ZipCode).HasMaxLength(150);
		}
	}
}