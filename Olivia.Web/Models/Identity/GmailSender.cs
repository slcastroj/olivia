using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace Olivia.Web.Models.Identity
{
    public class GmailSender : IEmailSender
    {
        public GmailSenderOptions Options { get; }
        public GmailSender(IOptions<GmailSenderOptions> opt)
        {
            Options = opt.Value;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using (var msg = new MailMessage())
            {
                msg.To.Add(email);
                msg.From = new MailAddress(Options.GmailUser);
                msg.Subject = subject;
                msg.Body = htmlMessage;
                msg.IsBodyHtml = true;

                using (var c = new SmtpClient("smtp.gmail.com"))
                {
                    c.Port = 587;
                    c.Credentials = new NetworkCredential(Options.GmailUser, Options.GmailPassword);
                    c.EnableSsl = true;
                    c.Send(msg);
                    return Task.CompletedTask;
                }
            }
        }
    }
}