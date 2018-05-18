using System.Net;
using System.Net.Mail;

namespace PB.BL.Senders
{
    public class GmailSender
    {
        static GmailSender()
        {
            GmailHost = "smtp.gmail.com";
            GmailPort = 587; // Gmail can use ports 25, 465 & 587; but must be 25 for medium trust environment.
            GmailSSL = true;
            //GmailUsername = "poba.politiekebarometer@gmail.com";
            GmailUsername = "poba.TeamNameNotFoundException@gmail.com";
            GmailPassword = "politiekebarometer1";
        }

        public static string GmailUsername { get; set; }
        public static string GmailPassword { get; set; }
        public static string GmailHost { get; set; }
        public static int GmailPort { get; set; }
        public static bool GmailSSL { get; set; }

        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }

        public void Send()
        {
            SmtpClient smtp = new SmtpClient
            {
                Host = GmailHost,
                Port = GmailPort,
                EnableSsl = GmailSSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(GmailUsername, GmailPassword)
            };

            using (var message = new MailMessage(GmailUsername, ToEmail))
            {
                message.Subject = Subject;
                message.Body = Body;
                message.IsBodyHtml = IsHtml;
                smtp.Send(message);
            }
        }


        #region HTML Body

        public static readonly string DefaultItemIcon =
            "https://integratieproject.azurewebsites.net/Content/Images/Users/user.png";

        public static readonly string DefaultUsernameSubstring = "##USER##";
        public static readonly string DefaultItemLinkSubstring = "##ITEMLINK##";

        public static readonly string WeeklyReviewListSubstring = "##LIST##";
        public static readonly string WeeklyReviewListItemIconSubstring = "##ICONLINK##";
        public static readonly string WeeklyReviewListItemNameSubstring = "##ITEMNAAM##";
        public static readonly string WeeklyReviewListItemDescriptionSubstring = "##ALERTDESCRIPTION##";


        public static string WeeklyReviewListItem =
            @"<li style='margin: 0 0 25px 0;padding: 15px;width:-webkit-fill-available;width:-moz-available;width:fill-available;overflow: hidden; list-style: none !important;display: inline-flex;background: #efefef;'>
                <img src='" + WeeklyReviewListItemIconSubstring +
            @"' style='margin-right: 15px; max-width: 10%; max-height: 100px; width: auto; height: auto; object-fit: contain;'>
                <div>
                    <h4 style='margin-top: 10.5px;'><a href='" + DefaultItemLinkSubstring +
            @"' style='margin: 0; font-size: 1.5em; text-decoration: none;'>" + WeeklyReviewListItemNameSubstring +
            @" </a></h4>
                    <p style='margin: 0;padding: 0;margin-bottom: 10px;font-weight: 500;color: #8191ab;font-size: 1.2em;line-height: 1.6;'>" +
            WeeklyReviewListItemDescriptionSubstring + @"</p>
                </div>
            </li>";

        public static string WeeklyReviewBody =
            @"
            <link href='https://fonts.googleapis.com/css?family=Convergence' rel='stylesheet'>
            <div style='margin: 0; padding: 10px 5%; height: 100%; background-color: #eceeed;'>
                <div style='font-family: Convergence, " + '"' + "Open Sans" + '"' +
            @", Arial, sans-serif;margin-bottom: 0;width: 100%;min-height: 100px;display: table;border-collapse: collapse;border-spacing: 0;border-top-left-radius: 25px;border-top-right-radius: 25px;background-color:#00bfff;text-align: center;'>
                    <h1 style='margin: 0;display: table-cell;vertical-align: middle;font-weight: 300;font-size: 5em;font-variant: all-small-caps;color: aliceblue;'> Weekly review</h1>
                </div>
                <div style='padding: 30px; background-color: #fff'>
                    <h2 style='margin: 0 0 15px 0;padding: 0;line-height: 1.4;font-weight: 500;font-size: 40px;border-bottom: #3f526d 2px solid;color: #3f526d;'>
                        Uw weekly review!
                    </h2>
                    <h3 style='line-height: 1.4;color: #3f526d;font-weight: 500;font-size: 20px;margin: 20px 0 15px;padding: 0;'>
                        Hey " + DefaultUsernameSubstring + @"!
                    </h3>
                    <p style='margin: 0;padding: 0;margin-bottom: 10px;font-weight: 500;color: #96a6b0;font-size: 1.2em;line-height: 1.6;'>
                        Welkom op jouw eigen volledig gepersonaliseerde weekly review. Een persoonlijke terugkijk naar wat er
                        afgelopen week gebeurd is op ons platform en geven we je de topmomenten in een overzichtelijke weergave af.
                        Hierin tonen we je de dingen waarin jij geïnteresserd bent.</p>
                    <p style='margin: 0;padding: 0;margin-bottom: 10px;font-weight: 500;color: #96a6b0;font-size: 1.2em;line-height: 1.6;'>...</ p>
                    <p style='margin: 0;padding: 0;margin-bottom: 10px;font-weight: 500;color: #96a6b0;font-size: 1.2em;line-height: 1.6;'> Waar zit je nog op te wachten? Er valt heel wat te ontdekken hoor!</p>
                    <hr style='margin: 20px 0;'>
                    <ul style='margin: 0;padding: 0;'>
                        <li style='margin: 0;padding: 15px;width:-webkit-fill-available;width:-moz-available;width:fill-available;overflow: hidden; list-style: none !important;display: inline-flex;background: #efefef;'>
                            <img src='https://i.imgur.com/XtSoD3S.png' style='margin-right: 15px; max-width: 10%; max-height: 100px; width: auto; height: auto; object-fit: contain;'>
                            <div class='item-info'>
                                <h4><a href='" + DefaultItemLinkSubstring +
            @"' style='margin: 0; font-size: 1.5em; text-decoration: none;'>" + WeeklyReviewListItemNameSubstring +
            @"</a></h4>
                                <p style='margin: 0;padding: 0;margin-bottom: 10px;font-weight: 500;color: #8191ab;font-size: 1.2em;line-height: 1.6;'>
                                    " + WeeklyReviewListItemDescriptionSubstring + @"
                                </p>
                            </div>
                        </li>
                    </ul>
                    <hr style='margin: 20px 0;'>
                    <ul style='margin: 0;padding: 0;'>
                        " + WeeklyReviewListSubstring + @"
                    </ul>
                </div>
                <div style='font-family: Convergence, " + '"' + "Open Sans" + '"' +
            @", Arial, sans-serif;margin-bottom: 0;width: 100%;min-height: 100px;display: table;border-collapse: collapse;border-spacing: 0;border-bottom-left-radius: 25px;border-bottom-right-radius: 25px;background-color: #96a6b0;text-align: center;'>
                    <ul style='margin: 0;padding: 0;display: table-cell;vertical-align: middle;text-align: center;' >
                        <li style='margin: 0 50px;display: inline-block;list-style-type: none;'><a href = '#' style='color: #3f526d;'>Unsubscribe</a></li>
                        <li style='margin: 0 50px;display: inline-block;list-style-type: none;'><a href='#' style='color: #3f526d;'>Contact</a></li>
                    </ul>
                </div>
            </div>";

        #endregion
    }
}