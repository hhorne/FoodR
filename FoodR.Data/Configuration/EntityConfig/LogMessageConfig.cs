using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodR.Data.Models;

namespace FoodR.Data.Configuration.EntityConfig
{
	public class LogMessageConfig : EntityConfig<LogMessage>
	{
		public LogMessageConfig()
		{
			Property(l => l.Thread).HasMaxLength(255);
			Property(l => l.Level).HasMaxLength(50);
			Property(l => l.Logger).HasMaxLength(255);
			Property(l => l.Message).HasMaxLength(4000);
			Property(l => l.Exception).HasMaxLength(2000);
		}
	}
}
