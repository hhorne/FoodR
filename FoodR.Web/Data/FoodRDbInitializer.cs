using System.Data.Entity;
using System.Data.Entity.Migrations;
using FoodR.Web.Data.Models;

namespace FoodR.Web.Data
{
	internal class FoodRDbInitializer : MigrateDatabaseToLatestVersion<FoodRContext, FoodRDbConfig>
	{
		public override void InitializeDatabase(FoodRContext context)
		{
			InitializeTrucks(context);

			base.InitializeDatabase(context);
		}

		private void InitializeTrucks(FoodRContext context)
		{
			context.FoodTrucks.AddOrUpdate(t => t.Name,
				new []
				{
					new FoodTruck
					{
						Name = "Happy Grilled Cheese",
						Description = "Specialty Grilled Cheese Sandwiches",
					},
					new FoodTruck
					{
						Name = "On The Fly",
						Description = "Sandwiches & Stuff. Available for Catering and Private Parties.",
					}
				});
		}
	}
}