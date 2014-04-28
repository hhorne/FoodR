using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodR.Data.Models
{
	public class City
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public virtual State State { get; set; }
		public virtual ICollection<Area> Areas { get; set; } 
	}
}
