using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace FoodR.Web.Data.Models
{
	public class Location
	{
		public int LocationId { get; set; }
		public string Name { get; set; }
		public Address Address { get; set; }
		public DbGeography Coordinates { get; set; }
	}
}