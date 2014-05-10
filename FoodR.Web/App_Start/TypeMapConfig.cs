using System.Web.Mvc;
using AutoMapper;
using FoodR.Data.Models;
using FoodR.Web.Controllers;
using FoodR.Web.ViewModels;
using FoodR.Web.Services;
using System.Collections.Generic;
using System;

namespace FoodR.Web
{
	public class TypeMapConfig
	{
		public static void RegisterTypeMaps()
		{

			Mapper.CreateMap<FoodTruck, TruckListDetailViewModel>();
			Mapper.CreateMap<FoodTruck, TruckDetailsViewModel>();
			Mapper.CreateMap<FoodTruck, TruckEditViewModel>();
			Mapper.CreateMap<TruckEditViewModel, FoodTruck>();
			Mapper.CreateMap<KeyValuePair<DateTime, IEnumerable<ScheduledStop>>, ScheduleDay>()
				.ForMember(dest => dest.Day, opt => opt.MapFrom(src => src.Key))
				.ForMember(dest => dest.Entries, opt => opt.MapFrom(src => src.Value));
			
			Mapper.CreateMap<ScheduleDay, DailyScheduleViewModel>();
			Mapper.CreateMap<IEnumerable<ScheduleDay>, IEnumerable<DailyScheduleViewModel>>();
			Mapper.CreateMap<ScheduledStop, ScheduledStopDetailsViewModel>();
			Mapper.CreateMap<ScheduledStop, ScheduledStopEditViewModel>();
			Mapper.CreateMap<ScheduledStopEditViewModel, ScheduledStop>();
			
			Mapper.CreateMap<Location, LocationDetailViewModel>();
			
			Mapper.CreateMap<Location, LocationEditViewModel>();
			Mapper.CreateMap<LocationEditViewModel, Location>();
			Mapper.CreateMap<Location, SelectListItem>()
				.ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id));
			
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