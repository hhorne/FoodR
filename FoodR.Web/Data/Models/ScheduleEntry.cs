using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FoodR.Web.Data.Models
{
	public class ScheduleEntry
	{
		public int ScheduleEntryId { get; set; }
		
		public bool Active { get; set; }
		public string Comments { get; set; }
		
		public int FoodTruckId { get; set; }
		public virtual FoodTruck FoodTruck { get; set; }
		
		public int LocationId { get; set; }
		public virtual Location Location { get; set; }
		
		public DateTime From { get; set; }
		public DateTime To { get; set; }	
		
		public DateTime LastModifiedOn { get; set; }
		public virtual FoodRUser LastModifiedBy { get; set; }
		
		public DateTime CreatedOn { get; set; }
		public virtual FoodRUser CreatedBy { get; set; }
	}
}