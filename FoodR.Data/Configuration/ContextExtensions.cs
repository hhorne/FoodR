using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Data.Entity;
using System.Reflection;
using FoodR.Data.Configuration;

public static class ContextExtensions
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "context", Justification="This is helper extension method on DbContext.")]
	public static void ComposeModelConfiguration(this DbContext context, DbModelBuilder modelBuilder)
	{
		var contextConfiguration = new ContextConfiguration();
		using (var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly()))
		{
			using (var container = new CompositionContainer(catalog))
			{
				container.ComposeParts(contextConfiguration);

				foreach (var configuration in contextConfiguration.Configurations)
				{
					configuration.AddConfiguration(modelBuilder.Configurations);
				}
			}
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
