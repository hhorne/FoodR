using System;
using FoodR.Web.Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace FoodR.Web.Data
{
	public class FoodRContext : IdentityDbContext<FoodRUser>
	{
		public DbSet<Badge> Badges { get; set; }
		public DbSet<EarnedBadge> EarnedBadges { get; set; }
		public DbSet<FoodTruck> FoodTrucks { get; set; }
		public DbSet<Location> Locations { get; set; }
		public DbSet<Event> Events { get; set; }
		public DbSet<Menu> Menus { get; set; }

		public FoodRContext() : base("FoodR", throwIfV1Schema: false)
		{
			Database.Initialize(true);
		}

		static FoodRContext()
		{
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
			Database.SetInitializer(new DropCreateDatabaseIfModelChanges<FoodRContext>());
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

		protected override void Seed(FoodRContext context)
		{
			InitializeTrucks(context);
			InitializeIdentityForEF(context);
			base.Seed(context);
		}

		private void InitializeTrucks(FoodRContext context)
		{
			
			context.FoodTrucks.AddOrUpdate(t => t.Name, new[]
				{
					new FoodTruck
					{
						Name = "Happy Grilled Cheese",
						Description = "Specialty Grilled Cheese Sandwiches",
						CreatedOn = DateTime.Now,
						LastModifiedOn = DateTime.Now,
						Menus = new [] {
							new Menu
							{
								Name = "Grilled Cheese Menu"
							}
						},
						Events = new [] { 
							new Event
							{
								From = new DateTime(2014, 4, 23, 11, 0, 0),
								To = new DateTime(2014, 4, 23, 14, 0, 0),
								Location = new Location
								{
									Name = "Av Med",
									Address = new Address() {
										StreetAddress = "123 Main St"
									}
								},
								LastModifiedOn = DateTime.Now
							}
						}
					},
					new FoodTruck
					{
						Name = "On The Fly",
						Description = "Sandwiches & Stuff. Available for Catering and Private Parties.",
						CreatedOn = DateTime.Now,
						LastModifiedOn = DateTime.Now,
						Events = new [] { 
							new Event
							{
								From = new DateTime(2014, 4, 23, 11, 0, 0),
								To = new DateTime(2014, 4, 23, 14, 0, 0),
								Location = new Location
								{
									Name = "Courthouse",
									Address = new Address() {
										StreetAddress = "123 Main St"
									}
								},
								LastModifiedOn = DateTime.Now
							},
							new Event
							{
								From = new DateTime(2014, 4, 23, 18, 0, 0),
								To = new DateTime(2014, 4, 23, 22, 0, 0),
								Location = new Location
								{
									Name = "Arrd Wolf",
									Address = new Address() {
										StreetAddress = "123 Wolf St"
									}
								},
								LastModifiedOn = DateTime.Now
							}
						}
					},
					new FoodTruck
					{
						Name = "Taste Buds Express",
						Description = "TACOS!",
						CreatedOn = DateTime.Now,
						LastModifiedOn = DateTime.Now,
						Events = new [] { 
							new Event
							{
								From = new DateTime(2014, 4, 23, 11, 0, 0),
								To = new DateTime(2014, 4, 23, 14, 0, 0),
								Location = new Location
								{
									Name = "Forsyth an Main",
									Address = new Address() {
										StreetAddress = "123 Main St"
									}
								}, 
								LastModifiedOn = DateTime.Now
							}
						}
					}
				});

			//context.FoodTrucks.AddOrUpdate(t => t.Name,
			//	new[]
			//	{
			//		new FoodTruck
			//		{
			//			Name = "Happy Grilled Cheese",
			//			Description = "Specialty Grilled Cheese Sandwiches",
			//			CreatedOn = DateTime.Now,
			//			LastModifiedOn = DateTime.Now
			//		},
			//		new FoodTruck
			//		{
			//			Name = "On The Fly",
			//			Description = "Sandwiches & Stuff. Available for Catering and Private Parties.",
			//			CreatedOn = DateTime.Now,
			//			LastModifiedOn = DateTime.Now
			//		}
			//	});
		}

		public static void InitializeIdentityForEF(FoodRContext db)
		{
			var userManager = new FoodRUserManager(new UserStore<FoodRUser>(db));
			var roleManager = new FoodRRoleManager(new RoleStore<IdentityRole>(db));
			const string name = "admin@admin.com";
			const string password = "Admin@123456";
			const string roleName = "Admin";

			//Create Role Admin if it does not exist
			var role = roleManager.FindByName(roleName);
			if (role == null)
			{
				role = new IdentityRole(roleName);
				var roleresult = roleManager.Create(role);
			}

			var user = userManager.FindByName(name);
			if (user == null)
			{
				user = new FoodRUser { UserName = name, Email = name };
				var result = userManager.Create(user, password);
				result = userManager.SetLockoutEnabled(user.Id, false);
			}

			// Add user admin to Role Admin if not already added
			var rolesForUser = userManager.GetRoles(user.Id);
			if (!rolesForUser.Contains(role.Name))
			{
				var result = userManager.AddToRole(user.Id, role.Name);
			}
		}
	}
}