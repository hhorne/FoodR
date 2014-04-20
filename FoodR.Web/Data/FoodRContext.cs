﻿using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Data.Entity;
using System.Reflection;
using FoodR.Web.Data.EntityTypeConfiguration;
using FoodR.Web.Data.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FoodR.Web.Data
{
	public class FoodRContext : IdentityDbContext<FoodRUser>
	{
        public FoodRContext() : base("DefaultConnection", throwIfV1Schema: false) {
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

			foreach (var configuration in
				contextConfiguration.Configurations)
			{
				configuration.AddConfiguration(
					modelBuilder.Configurations);
			}
		}
	}
}