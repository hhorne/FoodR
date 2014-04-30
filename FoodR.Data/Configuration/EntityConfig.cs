using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace FoodR.Data.Configuration
{
	public abstract class EntityConfig<T> : EntityTypeConfiguration<T>, IEntityConfig where T : class
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