using AutoMapper;
using FoodR.Data.Models;
using FoodR.Web.Controllers;
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