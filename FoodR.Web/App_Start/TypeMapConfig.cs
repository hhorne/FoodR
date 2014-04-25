using AutoMapper;
using FoodR.Web.Controllers;
using FoodR.Web.Data.Models;
using FoodR.Web.ViewModels;

namespace FoodR.Web
{
	public class TypeMapConfig
	{
		public static void RegisterTypeMaps()
		{
			Mapper.CreateMap<FoodTruck, TruckDetailsViewModel>();
		}
	}
}