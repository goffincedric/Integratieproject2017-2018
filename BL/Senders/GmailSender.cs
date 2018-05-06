using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace PB.BL.Senders
{
    public class GmailSender
    {
        public static string GmailUsername { get; set; }
        public static string GmailPassword { get; set; }
        public static string GmailHost { get; set; }
        public static int GmailPort { get; set; }
        public static bool GmailSSL { get; set; }

        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }


        #region HTML Body
        public static readonly string DefaultItemIcon = "https://i.imgur.com/CNO0NKE.png";
        public static readonly string DefaultUsernameSubstring = "##USER##";
        public static readonly string DefaultItemLinkSubstring = "##ITEMLINK##";

        public static readonly string WeeklyReviewListSubstring = "##LIST##";
        public static readonly string WeeklyReviewListItemIconSubstring = "##ICONLINK##";
        public static readonly string WeeklyReviewListItemNameSubstring = "##ITEMNAAM##";
        public static readonly string WeeklyReviewListItemDescriptionSubstring = "##ALERTDESCRIPTION##";

        public static string WeeklyReviewListItem =
            @"<li>
                    <img src='" + WeeklyReviewListItemIconSubstring + @"' class='item-image'>
                    <div class='item-info'>
                        <h4><a href='" + DefaultItemLinkSubstring + @"'>" + WeeklyReviewListItemNameSubstring + @"</a></h4>
                        <p>" + WeeklyReviewListItemDescriptionSubstring + @"</p>
                    </div>
                </li>";
        public static string WeeklyReviewBody =
            @"
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <link href='https://fonts.googleapis.com/css?family=Convergence' rel='stylesheet'>
                <style>
                    #body {
                        margin: 0;
                        padding: 10px 5%;
                        background-color: #eceeed;
                        font-family: 'Open Sans', Arial, sans-serif;
                    }

                    .header-text {
                        font-family: Convergence, 'Open Sans', Arial, sans-serif;
                        margin-bottom: 0;
                        width: 100%;
                        min-height: 100px;

                        display: table;
                        border-collapse: collapse;
                        border-spacing: 0;
                        border-top-left-radius: 25px;
                        border-top-right-radius: 25px;

                        background-color: #00bfff;

                        text-align: center;
                    }

                    .header-text > h1 {
                        margin: 0;

                        display: table-cell;
                        vertical-align: middle;

                        font-weight: 300;
                        font-size: 75px;
                        font-variant: all-small-caps;
                        color: aliceblue;
                    }

                    # content {
                        padding: 30px;

                        background-color: #fff;
                        min-height: 100px;
                    }

                    h2 {
                        margin: 0 0 15px 0;
                        padding: 0;

                        line-height: 1.4;
                        font-weight: 500;
                        font-size: 40px;
                        border-bottom: #3f526d 2px solid;
                        color: #3f526d;
                    }

                    h3 {
                        line-height: 1.4;
                        color: #3f526d;
                        font-weight: 500;
                        font-size: 20px;
                        margin: 20px 0 15px;
                        padding: 0;
                    }

                    p {
                        margin: 0;
                        padding: 0;
                        margin-bottom: 10px;
                        font-weight: 500;
                        color: #96a6b0;
                        font-size: 15px;
                        line-height: 1.6;
                        font-size: 1.2em;
                    }

                    hr {
                        margin: 20px 0;
                    }

                    ul {
                        margin: 0;
                        padding: 0;
                    }

                    .review-item > li {
                        margin-bottom: 25px;
                        padding: 15px;

                        list-style: none;
                        display: inline-flex;

                        background: #efefef;
                    }

                    .review-item.item-image {
                        margin-right: 15px;
                        max-width: 10%;
                        max-height: 100px;
                        width: auto;
                        height: auto;
                        object-fit: contain;
                    }

                    .review-item.item-info {
                        width: 85%;
                    }

                    .review-item.item-info > h4 {
                        font-size: 1.5em;
                        margin: 0;
                    }

                    .review-item.item-info > h4 > a {
                        text-decoration: none;
                        color: dimgray;
                    }

                    .review-item.item-info > h4 > a:hover {
                        color: lightslategray;
                        text-decoration: underline;
                    }

                    .footer-text {
                        font-family: Convergence, 'Open Sans', Arial, sans-serif;
                        margin-bottom: 0;
                        width: 100%;
                        min-height: 100px;

                        display: table;
                        border-collapse: collapse;
                        border-spacing: 0;
                        border-bottom-left-radius: 25px;
                        border-bottom-right-radius: 25px;

                        background-color: #96a6b0;

                        text-align: center;
                    }

                    .footer-text > h1 {
                        margin: 0;

                        font-weight: 300;
                        font-size: 75px;
                        font-variant: all-small-caps;
                        color: aliceblue;
                    }

                    .footer-text ul
                    {
                        display: table-cell;
                        vertical-align: middle;
                        text-align: center;
                    }

                    .footer-text li
                    {
                        margin: 0 50px;
                        display: inline-block;
                        list-style-type: none;
                    }

                    .footer-text li a {
                        color: #3f526d;
                    }
                </style>
            </head>
            <body>
                <div class='header-text'>
                    <h1>Weekly review</h1>
                </div>
                <div id='content'>
                    <h2>
                        Uw weekly review!
                    </h2>
                    <h3>
                        Hey " + DefaultUsernameSubstring + @"!
                    </h3>
                    <p>
                        Welkom op jouw eigen volledig gepersonaliseerde weekly review. Een persoonlijke terugkijk naar wat er
                        afgelopen week gebeurd is op ons platform en geven we je de topmomenten in een overzichtelijke weergave af.
                        Hierin tonen we je de dingen waarin jij geïnteresserd bent.</p>
                    <p>...</p>
                    <p>Waar zit je nog op te wachten? Er valt heel wat te ontdekken hoor!</p>
                    <hr>
                    <ul class='review-item'>
                        " + WeeklyReviewListSubstring + @"
                    </ul>
                </div>
                <div class='footer-text'>
                    <ul>
                        <li><a href='#'>Unsubscribe</a></li>
                        <li><a href='#'>Contact</a></li>
                    </ul>
                </div>
            </body>
        </html>";
        #endregion

        static GmailSender()
        {
            GmailHost = "smtp.gmail.com";
            GmailPort = 587; // Gmail can use ports 25, 465 & 587; but must be 25 for medium trust environment.
            GmailSSL = true;
            //GmailUsername = "poba.politiekebarometer@gmail.com";
            GmailUsername = "poba.TeamNameNotFoundException@gmail.com";
            GmailPassword = "politiekebarometer1";
        }

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
    }
}