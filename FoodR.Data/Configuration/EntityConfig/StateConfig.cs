using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodR.Data.Models;

namespace FoodR.Data.Configuration.EntityConfig
{
	public class StateConfig : EntityConfig<State>
	{
		public StateConfig()
		{
			HasKey(s => s.Id);
			Property(s => s.Id).HasMaxLength(2);
			Property(s => s.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			Property(s => s.Name).HasMaxLength(50);
		}
	}
}
