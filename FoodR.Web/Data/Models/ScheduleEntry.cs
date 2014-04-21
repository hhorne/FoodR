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
		[ForeignKey("Vendor")]
		public int VendorId { get; set; }
		public virtual Vendor Vendor { get; set; }

		[ForeignKey("Location")]
		public int LocationId { get; set; }
		public virtual Location Location { get; set; }

		[Key]
		public int EntryId { get; set; }

		public bool Active { get; set; }

		public DateTime From { get; set; }
		public DateTime To { get; set; }
		
		public DateTime LastModifiedOn { get; set; }
		public string LastModifiedBy { get; set; }
		public string ModifiedComments { get; set; }
	}
}