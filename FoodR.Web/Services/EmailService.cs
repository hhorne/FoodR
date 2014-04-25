using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace FoodR.Web.Services
{
	public class EmailService : IEmailService
	{
		public Task SendAsync(IdentityMessage message)
		{
			// Plug in your email service here to send an email.
			return Task.FromResult(0);
		}
	}

	public interface IEmailService : IIdentityMessageService
	{
	}
}