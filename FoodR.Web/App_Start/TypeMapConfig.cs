using System.Web.Mvc;
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
			Mapper.CreateMap<FoodTruck, TruckEditViewModel>();
			Mapper.CreateMap<TruckEditViewModel, FoodTruck>();
			
			Mapper.CreateMap<Location, LocationDetailViewModel>();
			
			Mapper.CreateMap<Location, LocationEditViewModel>();
			Mapper.CreateMap<LocationEditViewModel, Location>();
			
			Mapper.CreateMap<Area, AreaDetailViewModel>();
			
			Mapper.CreateMap<Area, SelectListItem>()
				.ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id));
			
			Mapper.CreateMap<City, SelectListItem>()
				.ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id));
			
			Mapper.CreateMap<State, SelectListItem>()
				.ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id));
			//.ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));
		}
	}
}