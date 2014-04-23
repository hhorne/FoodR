using System.Web;
using FoodR.Web.Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Ninject.Modules;

namespace FoodR.Web.InjectionModules
{
	public class IdentityModule : NinjectModule
	{
		public override void Load()
		{
			Bind<IUserStore<FoodRUser>>().To<UserStore<FoodRUser>>();
			Bind<FoodRUserManager>().ToSelf();
			Bind<IRoleStore<IdentityRole, string>>().To<RoleStore<IdentityRole, string, IdentityUserRole>>();
			Bind<FoodRRoleManager>().ToSelf();
			Bind<IAuthenticationManager>().ToMethod(p => HttpContext.Current.GetOwinContext().Authentication);
		}
	}
}