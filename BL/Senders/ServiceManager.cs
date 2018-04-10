using Microsoft.AspNet.Identity;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SendGrid.Helpers.Mail;
using PB.BL.Senders;


namespace PB.BL
{
  public class EmailService : IIdentityMessageService
  {
    public async Task SendAsync(IdentityMessage message)
    {
      var clientKey = ConfigurationManager.AppSettings["SendGridClient"];
      var client = new SendGridClient(clientKey);
      var from = new EmailAddress("Services@barometer.be", "Barometer");
      var subject = message.Subject;
      var to = new EmailAddress(message.Destination,"Barometer user");
      var plainTextContent = "";
      var htmlContent = message.Body;
      var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
      var response = await client.SendEmailAsync(msg);
    }
  }

  public class SmsService : IIdentityMessageService
  {
    private readonly ITwilioMessageSender _messageSender;

    public SmsService() : this(new TwilioMessageSender())
    {
    }

    public SmsService(ITwilioMessageSender messageSender)
    {
      _messageSender = messageSender;
    }

    public async Task SendAsync(IdentityMessage message)
    {
      await _messageSender.SendMessageAsync(message.Destination,
                                            ConfigurationManager.AppSettings["SenderID"],
                                            message.Body);
    }
  }
}

