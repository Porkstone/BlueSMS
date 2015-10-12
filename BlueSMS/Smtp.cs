using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BlueSMS
{
    class Smtp
    {
        public const string SMTP_Hostname = "smtp.mandrillapp.com";
        public const int SMTP_Port = 587;
        public const int SMTP_TimeoutMs = 10000;
        public const bool SMTP_UseSsl = true;

        public static void SendHtmlEmail(string subject, string bodyHtml)
        {
            SmtpClient client = new SmtpClient();
            client.Port = SMTP_Port;
            client.Host = SMTP_Hostname;
            client.EnableSsl = SMTP_UseSsl;
            client.Timeout = SMTP_TimeoutMs;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            Console.WriteLine("SMTP_Port: " + SMTP_Port);
            Console.WriteLine("SMTP_Hostname: " + SMTP_Hostname);
            
            client.Credentials = new System.Net.NetworkCredential(Config.SmtpUsername, Config.SmtpPassword);

            MailMessage mm = new MailMessage("BlueSms@BlueMotorFinance.co.uk", "PaymentReminders@bluemotorfinance.co.uk", subject, bodyHtml);
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.IsBodyHtml = true;

            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            client.Send(mm);
        }
    }
}
