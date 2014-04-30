using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace FoodR.Web.Services
{
	public class SmsService : ISmsService
	{
		public Task SendAsync(IdentityMessage message)
		{
			// Plug in your sms service here to send a text message.
			return Task.FromResult(0);
		}
	}

	public interface ISmsService : IIdentityMessageService
	{
	}
}