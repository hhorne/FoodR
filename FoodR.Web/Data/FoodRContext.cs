using FoodR.Web.Data.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FoodR.Web.Data
{
	public class FoodRContext : IdentityDbContext<FoodRUser>
	{
        public FoodRContext()
            : base("DefaultConnection", throwIfV1Schema: false) {
        }

		static FoodRContext()
		{
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
			// Database.SetInitializer<FoodRContext>(new FoodRDbInitializer());
        }

		public static FoodRContext Create()
		{
			return new FoodRContext();
        }
	}
}