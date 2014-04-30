using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Data.Entity;
using System.Reflection;
using FoodR.Data.Configuration;

public static class ContextExtensions
{
	public static void ComposeModelConfiguration(this DbContext context, DbModelBuilder modelBuilder)
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

	class ContextConfiguration
	{
		[ImportMany(typeof(IEntityConfig))]
		public IEnumerable<IEntityConfig> Configurations
		{
			get;
			set;
		}
	}
}
