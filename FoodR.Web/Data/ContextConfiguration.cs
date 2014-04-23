using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace FoodR.Web.Data
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