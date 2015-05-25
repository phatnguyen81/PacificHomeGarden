using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace pCMS.Framework.Helpers
{
    public static class EmailHelper
    {
        public static void SendMail(string email, string subject, string body)
        {
            var configurationFile = WebConfigurationManager.OpenWebConfiguration("~/web.config");
            var mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
            var client = new SmtpClient { Timeout = 60000 };
            using (var message = new MailMessage(mailSettings.Smtp.From, email)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                client.Send(message);
            }
        }
        public static void SendMailWithSignature(string email, string subject, string templatefile, params string[] list)
        {
            var configurationFile = WebConfigurationManager.OpenWebConfiguration("~/web.config");
            var mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
            var fromEmail = mailSettings.Smtp.From;
            var client = new SmtpClient { Timeout = 60000 };
            using (var message = new MailMessage(fromEmail, email)
            {
                Subject = subject,
                IsBodyHtml = true
            })
            {
                var body = FileHelper.GetTemplateFileContent(templatefile);
                var signature = FileHelper.GetTemplateFileContent("Signature.htm");
                body = string.Format(body, list);
                var logo = new LinkedResource(HttpContext.Current.Server.MapPath("~/Content/pictures/logo.jpg")) { ContentId = "logo" };
                var htmlView = AlternateView.CreateAlternateViewFromString(body + signature, null, "text/html");
                htmlView.LinkedResources.Add(logo);
                message.AlternateViews.Add(htmlView);
                client.Send(message);
            }
           
        }
        public static void SendMail(MailMessage message)
        {

            var client = new SmtpClient { Timeout = 60000 };
            client.Send(message);
        }
    }
}
