using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace FoodR.Web.Data.EntityTypeConfiguration
{
	public class ContextConfiguration
	{
		[ImportMany(typeof(IEntityConfig))]
		public IEnumerable<IEntityConfig> Configurations
		{
			get;
			set;
		}
	}
}