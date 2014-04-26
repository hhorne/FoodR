using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using FoodR.Web.Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

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
			this.ComposeModelConfiguration(modelBuilder);
			base.OnModelCreating(modelBuilder);
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
						Rating = 3,
						CreatedOn = DateTime.Now,
						LastModifiedOn = DateTime.Now,
						Menus = new [] {
							new Menu
							{
								Name = "Grilled Cheese Menu",
								MenuItems = new []
								{
									new MenuItem() {
										Name = "Grilled Cheese Sammich",
										Description = "Its the classic grilled cheese sandwich we all love.",
										Price = 6.99
									},
									new MenuItem() {
										Name = "Mac and Cheese Sammich",
										Description = "Its a grilled cheese sandwich with Macaroni n Cheese added to it!",
										Price = 7.99
									}
								}
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
								LastModifiedOn = DateTime.Now,
								Active = true
							}
						}
					},
					new FoodTruck
					{
						Name = "On The Fly",
						Description = "Sandwiches & Stuff. Available for Catering and Private Parties.",
						Rating = 5,
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
								LastModifiedOn = DateTime.Now,
								Active = true
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
								LastModifiedOn = DateTime.Now,
								Active = true
							}
						}
					},
					new FoodTruck
					{
						Name = "Taste Buds Express",
						Description = "TACOS!",
						Rating = 2,
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
								LastModifiedOn = DateTime.Now,
								Active = true
							}
						}
					}
				});
		}

		public static void InitializeIdentityForEF(FoodRContext db)
		{
			var userManager = new UserManager<FoodRUser>(new UserStore<FoodRUser>(db));
			var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
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