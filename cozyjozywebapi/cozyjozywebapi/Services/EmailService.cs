using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace cozyjozywebapi.Services
{
    public interface IEmailService
    {
        void SendEmail(string emailAddress, string subject, string body);
    }

    public class EmailService : IEmailService
    {
        private readonly string _emailHost;
        private readonly string _userName;
        private readonly string _password;

        public EmailService()
        {
            _emailHost = ConfigurationManager.AppSettings["emailHost"];
            _userName = ConfigurationManager.AppSettings["emailUserName"];
            _password = ConfigurationManager.AppSettings["emailPassword"];
        }
        public void SendEmail(string emailAddress, string subject, string body)
        {
            var client = new SmtpClient
            {
                Port = 25,
                Host = _emailHost,
                Timeout = 10000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(_userName, _password)
            };

            var mm = new MailMessage("donotreply@cozyjozy.net", emailAddress, subject, body)
            {
                BodyEncoding = Encoding.UTF8,
                DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
            };

            client.Send(mm);
        }
    }
}
