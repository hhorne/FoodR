using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FoodR.Web.Data.Models
{
	public class FoodTruck
	{
		[Key]
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

	}
}