using FoodR.Web.Services;
using Ninject.Modules;

namespace FoodR.Web.App_Start.InjectionModules
{
	public class ServiceModule : NinjectModule
	{
		public override void Load()
		{
			Bind<IEmailService>().To<EmailService>();
			Bind<ISmsService>().To<SmsService>();
			Bind<IUserService>().To<UserService>();
		}
	}
}