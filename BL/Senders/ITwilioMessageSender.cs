using System.Threading.Tasks;

namespace PB.BL.Senders
{
    public interface ITwilioMessageSender
  {
    Task SendMessageAsync(string to, string from, string body);
  }
}
