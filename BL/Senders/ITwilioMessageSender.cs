using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL.Senders
{
  public interface ITwilioMessageSender
  {
    Task SendMessageAsync(string to, string from, string body);
  }
}
