using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Web;

namespace FoodR.Web.Data.Models
{
	public class FoodTruck
	{
		public int Id { get; set; }

		public string Name { get; set; }
		public string Description { get; set; }
		public string Comments { get; set; }
		public int Rating { get; set; }

		//contact info
		public string Phone { get; set; }
		public string Phone2 { get; set; }
		public string Email { get; set; }
		public string Email2 { get; set; }
		public string Website { get; set; }
		public string Facebook { get; set; }
		public string Twitter { get; set; }

		public DateTime LastModifiedOn { get; set; }
		public string LastModifiedBy { get; set; }

		public DateTime CreatedOn { get; set; }
		public string CreatedBy { get; set; }

		public virtual ICollection<Event> Events { get; set; }

		public virtual ICollection<Menu> Menus { get; set; }
	}
}