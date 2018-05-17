using System.Configuration;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace PB.BL.Senders
{
    public class TwilioMessageSender : ITwilioMessageSender
    {
        public TwilioMessageSender()
        {
            TwilioClient.Init(ConfigurationManager.AppSettings["SMSAccountIdentification"],
                ConfigurationManager.AppSettings["SMSAccountPassword"]);
        }

        public async Task SendMessageAsync(string to, string from, string body)
        {
            await MessageResource.CreateAsync(new PhoneNumber(to),
                from: new PhoneNumber(from),
                body: body);
        }
    }
}