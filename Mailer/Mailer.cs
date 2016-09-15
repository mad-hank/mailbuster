using System;
using SendGrid;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Mail;
using System.Net.Http;
using System.Net;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.IO;

namespace Mailer
{
    public static class MailNotifier
    {
        public static async Task SendEmail(EmailContent vm)
        {
            // Create the email object first, then add the properties.
            SendGridMessage myMessage = new SendGridMessage();
            myMessage.AddTo(vm.To.Split(',').ToList());

            myMessage.Subject = vm.Subject;
            myMessage.Html = vm.Body;

            foreach (var item in vm.Attachments)
            {
                if (File.Exists(item))
                    myMessage.AddAttachment(item);
            }

            string username = ConfigurationManager.AppSettings["MailAccount"];
            string password = ConfigurationManager.AppSettings["Mailpassword"];
            string FromEmail = ConfigurationManager.AppSettings["EmailUser"];
            string FromEmailName = ConfigurationManager.AppSettings["EmailUserName"];

            myMessage.From = new MailAddress(FromEmail, FromEmailName);

            // Create credentials, specifying your user name and password.
            var credentials = new NetworkCredential(username, password);

            // Create an Web transport for sending email.
            var transportWeb = new Web(credentials);

            // Send the email.
            // You can also use the **DeliverAsync** method, which returns an awaitable task.
            try
            {
                // Send the email.
                if (transportWeb != null)
                {
                    await transportWeb.DeliverAsync(myMessage);
                }
                else
                {
                    Trace.TraceError("Failed to create Web transport.");
                    Task.FromResult(0);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message + " SendGrid probably not configured correctly.");
            }

        }

        //public static async Task SendEmail(EmailContent vm)
        //{
        //    // Create the email object first, then add the properties.
        //    //Api Key=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
        //    string username = ConfigurationManager.AppSettings["MailAccount"];
        //    string password = ConfigurationManager.AppSettings["Mailpassword"];
        //    string FromEmail = ConfigurationManager.AppSettings["EmailUser"];
        //    string FromEmailName = ConfigurationManager.AppSettings["EmailUserName"];

        //    MailMessage mailMsg = new MailMessage();

        //    // To
        //    mailMsg.To.Add(vm.To);

        //    // From
        //    mailMsg.From = new MailAddress(FromEmail, FromEmailName);

        //    // Subject and multipart/alternative Body
        //    mailMsg.Subject = vm.Subject;
        //    string text = vm.Body;
        //    //string html = @"<p>html body</p>";
        //    //mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
        //    mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Html));

        //    // Init SmtpClient and send
        //    SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
        //    System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(username, password);
        //    smtpClient.Credentials = credentials;

        //    smtpClient.SendAsync(mailMsg,1);
        //}

        //public static async Task SendEmail(EmailContent vm)
        //{
        //    string apiKey = Environment.GetEnvironmentVariable("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");
        //    dynamic sg = new SendGridAPIClient(apiKey);

        //    string username = ConfigurationManager.AppSettings["MailAccount"];
        //    string password = ConfigurationManager.AppSettings["Mailpassword"];
        //    string FromEmail = ConfigurationManager.AppSettings["EmailUser"];
        //    string FromEmailName = ConfigurationManager.AppSettings["EmailUserName"];

        //    Email from = new Email(FromEmail);
        //    string subject = vm.Subject;
        //    Email to = new Email(vm.To);
        //    Content content = new Content("text/plain", vm.Body);
        //    Mail mail = new Mail(from, subject, to, content);

        //    dynamic response = await sg.client.mail.send.post(requestBody: mail.Get());

        //    if(response!=null)
        //    {
        //        Debug.WriteLine(response);
        //        int i = 0;
        //        i++;
        //    }
        //}
    }
}