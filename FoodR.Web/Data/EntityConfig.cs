using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Web;

namespace FoodR.Web.Data.EntityTypeConfiguration
{
	public abstract class EntityConfig<T> :  EntityTypeConfiguration<T> where T : class
	{
		public void AddConfiguration(ConfigurationRegistrar registrar)
		{
			registrar.Add(this);
		}
	}

	public interface IEntityConfig
	{
		void AddConfiguration(ConfigurationRegistrar registrar);
	}
}