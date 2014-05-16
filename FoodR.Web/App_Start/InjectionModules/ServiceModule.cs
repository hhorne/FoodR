using AutoMapper;
using FoodR.Data;
using FoodR.Web.Services;
using Ninject.Modules;

namespace FoodR.Web.InjectionModules
{
	public class ServiceModule : NinjectModule
	{
		public override void Load()
		{
			Bind<IEmailService>().To<EmailService>();
			Bind<ISmsService>().To<SmsService>();
			Bind<IUserService>().To<UserService>();
			Bind<ITruckService>().To<TruckService>();
			Bind<IScheduleService>().To<ScheduleService>();
			Bind<IRepository>().To<FoodRRepository>();
			Bind<IMappingEngine>().ToMethod(x => Mapper.Engine);
		}
	}
}