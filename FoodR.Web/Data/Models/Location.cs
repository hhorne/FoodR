
namespace FoodR.Web.Data.Models
{
	public class Location
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public Address Address { get; set; }
		//public DbGeography Coordinates { get; set; }
	}
}