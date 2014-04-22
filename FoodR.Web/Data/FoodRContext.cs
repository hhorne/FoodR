using FoodR.Web.Data.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Reflection;

namespace FoodR.Web.Data
{
	public class FoodRContext : IdentityDbContext<FoodRUser>
	{
		public DbSet<Badge> Badges { get; set; }
		public DbSet<FoodTruck> FoodTrucks { get; set; }
		public DbSet<Location> Locations { get; set; }
		public DbSet<FoodRUser> Users { get; set; }
		public DbSet<ScheduleEntry> ScheduleEntries { get; set; }

        public FoodRContext() : base("FoodR", throwIfV1Schema: false) { }

		static FoodRContext()
		{
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
			Database.SetInitializer(new FoodRDbInitializer());
        }

		public static FoodRContext Create()
		{
			return new FoodRContext();
        }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			ComposeModelConfiguration(modelBuilder);

			base.OnModelCreating(modelBuilder);
		}

		private void ComposeModelConfiguration(DbModelBuilder modelBuilder)
		{
			var contextConfiguration = new ContextConfiguration();
			var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
			var container = new CompositionContainer(catalog);
			container.ComposeParts(contextConfiguration);

			foreach (var configuration in contextConfiguration.Configurations)
			{
				configuration.AddConfiguration(modelBuilder.Configurations);
			}
		}
	}

	internal sealed class FoodRDbConfig : DbMigrationsConfiguration<FoodRContext>
	{
		public FoodRDbConfig()
		{
			AutomaticMigrationsEnabled = true;
		}
	}
}