using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security.DataHandler.Serializer;

namespace FoodR.Data.Models
{
	public class State
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public virtual ICollection<City> Cities { get; set; }
	}
}
