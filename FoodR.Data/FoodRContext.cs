using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using FoodR.Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FoodR.Data
{
	public class FoodRContext : IdentityDbContext<FoodRUser>
	{
		public DbSet<Area> Areas { get; set; }
		public DbSet<Badge> Badges { get; set; }
		public DbSet<EarnedBadge> EarnedBadges { get; set; }
		public DbSet<FoodTruck> FoodTrucks { get; set; }
		public DbSet<Location> Locations { get; set; }
		public DbSet<ScheduleEntry> ScheduleEntries { get; set; }
		public DbSet<RecurringStop> RecurringStops { get; set; }
		public DbSet<Menu> Menus { get; set; }
		public DbSet<City> Cities { get; set; }
		public DbSet<State> States { get; set; }
		public DbSet<LogMessage> LogMessages { get; set; }

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
			modelBuilder.ComposeModelConfiguration();
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
			InitializeCitiesAndStates(context);
			InitializeAreasAndLocations(context);
			InitializeTrucks(context);
			InitializeMenuItems(context);
			InitializeIdentityForEF(context);
			base.Seed(context);
		}

		private void InitializeCitiesAndStates(FoodRContext context)
		{
			//var stateIds = new[]
			//{
			//	"AL",
			//	"AK",
			//	"AZ",
			//	"AR",
			//	"CA",
			//	"CO",
			//	"CT",
			//	"DE",
			//	"FL",
			//	"GA",
			//	"HI",
			//	"ID",
			//	"IL",
			//	"IN",
			//	"IA",
			//	"KS",
			//	"KY",
			//	"LA",
			//	"ME",
			//	"MD",
			//	"MA",
			//	"MI",
			//	"MN",
			//	"MS",
			//	"MO",
			//	"MT",
			//	"NE",
			//	"NV",
			//	"NH",
			//	"NJ",
			//	"NM",
			//	"NY",
			//	"NC",
			//	"ND",
			//	"OH",
			//	"OK",
			//	"OR",
			//	"PA",
			//	"RI",
			//	"SC",
			//	"SD",
			//	"TN",
			//	"TX",
			//	"UT",
			//	"VT",
			//	"VA",
			//	"WA",
			//	"WV",
			//	"WI",
			//	"WY",
			//}; // I really need to move this db initialization data to files or something...

			//var states = stateIds.Select(state => new State { Id = state }).ToList();
			//context.Set<State>().AddRange(states);
			//context.SaveChanges();

			//var fl = context.Set<State>().SingleOrDefault(s => s.Id.Equals("FL", StringComparison.OrdinalIgnoreCase));
			var fl = new State()
			{
				Id = "FL",
				Name = "Florida",
				Cities = new Collection<City>
					{
						new City { Name = "Jacksonville" },
					}
			};
			//fl.Cities = new Collection<City>
			//{
			//	new City { Name = "Jacksonville" },
			//};

			context.States.AddOrUpdate(s => s.Id, fl);

			context.SaveChanges();
		}

		private void InitializeAreasAndLocations(FoodRContext context)
		{
			var jacksonville = context.Cities.Single(c => c.Name.Equals("Jacksonville", StringComparison.OrdinalIgnoreCase));
			
			var southBank = new Area {
				Name = "Southbank",
				Description = "The Southbank area borders SanMarco and has locations like Aardwolf.",
				City = jacksonville,
				Locations = new List<Location> {
					new Location {	
						Name = "Aardwolf",
						UrlSlug = "Aardwolf",
						StreetAddress = "123 Wolf St"
					},
					new Location {
						Name = "AvMed",
						UrlSlug =  "AvMed",
						StreetAddress = "123 Main St"
					}
				}
			};

			var downTown = new Area {
				Name = "Downtown",
				Description = "A regular host to a wide variety of trucks",
				City = jacksonville,
				Locations = new List<Location> {
					new Location {
						Name = "Courthouse",
						UrlSlug = "Courthouse",
						StreetAddress = "123 Main St"
					},
					new Location {
						Name = "Forsyth and Main",
						UrlSlug = "Forsyth_and_Main",
						StreetAddress = "123 Main St"
					}
				}
			};
			
			context.Areas.AddOrUpdate(a => a.Name, new []
			{
				southBank,
				downTown,
			});

			context.SaveChanges();
		}

		private void InitializeTrucks(FoodRContext context)
		{
			var aardwolf = context.Locations.Single(l => l.UrlSlug.Equals("Aardwolf", StringComparison.OrdinalIgnoreCase));
			var avMed = context.Locations.Single(l => l.UrlSlug.Equals("AvMed", StringComparison.OrdinalIgnoreCase));
			var courthouse = context.Locations.Single(l => l.UrlSlug.Equals("Courthouse", StringComparison.OrdinalIgnoreCase));
			var forsyth = context.Locations.Single(l => l.UrlSlug.Equals("Forsyth_and_Main", StringComparison.OrdinalIgnoreCase));

			context.FoodTrucks.AddOrUpdate(t => t.Name, new[]
				{
					new FoodTruck
					{
						Name = "Happy Grilled Cheese",
						UrlSlug = "Happy_Grilled_Cheese",
						Description = "Specialty Grilled Cheese Sandwiches",
						Rating = 3,
						Facebook = "pages/The-Happy-Grilled-Cheese/284780458300232",
						ScheduleEntries = new [] { 
							new ScheduleEntry
							{
								From = new DateTime(2014, 4, 23, 11, 0, 0),
								To = new DateTime(2014, 4, 23, 14, 0, 0),
								Location = avMed,
								Active = true
							}
						}
					},
					new FoodTruck
					{
						Name = "On The Fly!",
						UrlSlug = "On_The_Fly",
						Description = "Sandwiches & Stuff. Available for Catering and Private Parties.",
						Rating = 5,
						Facebook = "Ontheflyjax",
						ScheduleEntries = new [] { 
							new ScheduleEntry
							{
								From = new DateTime(2014, 4, 23, 11, 0, 0),
								To = new DateTime(2014, 4, 23, 14, 0, 0),
								Location = courthouse,
								Active = true
							},
							new ScheduleEntry
							{
								From = new DateTime(2014, 4, 23, 18, 0, 0),
								To = new DateTime(2014, 4, 23, 22, 0, 0),
								Location = aardwolf,
								Active = true
							}
						}
					},
					new FoodTruck
					{
						Name = "Taste Buds Express",
						UrlSlug = "Taste_Buds_Express",
						Description = "TACOS!",
						Rating = 2,
						Email = "abc@abc.comx",
						Phone = "(904) 555-5555",
						Website = "www.google.com",
						Facebook = "TasteBudsExpress",
						ScheduleEntries = new [] { 
							new ScheduleEntry
							{
								From = new DateTime(2014, 4, 23, 11, 0, 0),
								To = new DateTime(2014, 4, 23, 14, 0, 0),
								Location = forsyth, 
								Active = true
							}
						}
					}
				});

			context.SaveChanges();
		}

		private void InitializeMenuItems(FoodRContext context)
		{
			var happy = context.FoodTrucks.Single(t => t.UrlSlug.Equals("Happy_Grilled_Cheese", StringComparison.OrdinalIgnoreCase));
			happy.Menus = new Collection<Menu> {
				new Menu {
					FoodTruck = happy,
					Name = "Grilled Cheese Menu",
					MenuItems = new Collection<MenuItem> {
						new MenuItem {
							Name = "Grilled Cheese Sammich",
							Description = "Its the classic grilled cheese sandwich we all love.",
							Price = 6.99
						},
						new MenuItem {
							Name = "Mac and Cheese Sammich",
							Description = "Its a grilled cheese sandwich with Macaroni n Cheese added to it!",
							Price = 7.99
						},
					}
				},
			};
			context.SaveChanges();
		}

		public static void InitializeIdentityForEF(FoodRContext db)
		{
			using (var userStoreContext = new UserStore<FoodRUser>(db))
			{
				using (var userManager = new UserManager<FoodRUser>(userStoreContext))
				{
					using (var roleStoreContext = new RoleStore<IdentityRole>(db))
					{
						using (var roleManager = new RoleManager<IdentityRole>(roleStoreContext))
						{
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
			}
		}
	}
}