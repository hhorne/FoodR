﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodR.Data.Models
{
	public class LogMessage
	{
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public string Thread { get; set; }
		public string Level { get; set; }
		public string Logger { get; set; }
		public string Exception { get; set; }
		public string Message { get; set; }

		public LogMessage()
		{
			Date = DateTime.Now;
		}
	}
}
