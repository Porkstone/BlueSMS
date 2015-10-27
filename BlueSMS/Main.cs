using CLAP;
using CLAP.Validation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlueSMS
{
    
    
    class Main
    {
        const string TestMessage = "Your next Blue Motor Finance loan repayment is due in 3 days on 12/10/2015. Please ensure funds are available. If you have any issues please call 020 3005 9332";

        [Empty, Help]
        public static void Help(string help)
        {
            // this is an empty handler that prints
            // the automatic help string to the console.
            Console.WriteLine(help);
        }

        [Verb(Aliases = "-testsms")]
        public static void TestSms(string text)
        {
            Console.WriteLine(text);
            Console.WriteLine("Sending Test SMS Message");

            Db.DbConnectionString = Config.DatabaseConnString;
            TwilioLib.AccountSid = Config.TwilioAccountSid;
            TwilioLib.AuthToken = Config.TwilioAuthToken;
            TwilioLib.FromNumber = Config.TwilioFromNumber;

            const string SendToNumber = "+447770928922";
            TwilioLib.SendSms(TestMessage, SendToNumber);
        }



        [Verb(Aliases="-encrypt", Description="encrypt -Text:\"TEST\"")]
        public static void Encrypt(string text)
        {
            Console.WriteLine(text);
            Console.WriteLine("Encrytped Value");
            Console.WriteLine(ConfigProtect.Encrypt(text));
            
            Clipboard.SetText(ConfigProtect.Encrypt(text));
            Console.WriteLine("Encrytped Value has been copied to the clipboard");
            Console.WriteLine("Decrypting to test");
            Console.WriteLine(ConfigProtect.Decrypt(ConfigProtect.Encrypt(text)));
        }


        [Verb(IsDefault = true, Aliases="-sendReport")]
        public static void PaymentReminders()
        {
            const string SmsMessagePt1 = "Your next Blue Motor Finance loan repayment is due in 3 days on ";
            const string SmsMessagePt2 = ". Please ensure funds are available. If you have any issues please call 020 3005 9332";
            Console.WriteLine("Processing Payment Reminders");
            Db.DbConnectionString = Config.DatabaseConnString;
            string reminderListHtml = "<p>SMS Payment Reminders will be sent to the following customers at 3pm tomorrow</p><ul>";
            var reminders = Db.GetPaymentRemindersReport();
                foreach (var item in reminders)
                {
                    reminderListHtml = reminderListHtml + "<li>" + item.AgreementReference + " - " + item.Forename + " " + item.Surname + " - BespokeArrears: " + item.BespokeArrearsState + " - " + SmsMessagePt1 + item.NextDueDate.ToShortDateString() + SmsMessagePt2 + "</li>";
                }
                reminderListHtml = reminderListHtml + "</ul><ul>";

            Smtp.SendHtmlEmail("SMS Payment Reminders Report", reminderListHtml);
	    }

        [Verb(Aliases = "-sendMessages")]
        public static void SendPaymentReminders()
        {
            const string SmsMessagePt1 = "Your next Blue Motor Finance loan repayment is due in 3 days on ";
            const string SmsMessagePt2 = ". Please ensure funds are available. If you have any issues please call 020 3005 9332";
            Console.WriteLine("Sending Payment Reminders");
            Db.DbConnectionString = Config.DatabaseConnString;
            TwilioLib.AccountSid = Config.TwilioAccountSid;
            TwilioLib.AuthToken = Config.TwilioAuthToken;
            TwilioLib.FromNumber = Config.TwilioFromNumber;

            string smsMessage = "";
            string smsNumber = "";
            var reminders = Db.GetPaymentReminders();
            foreach (var item in reminders)
            {
                smsMessage = SmsMessagePt1 + item.NextDueDate.ToShortDateString() + SmsMessagePt2;
                smsNumber = item.MobilePhoneStdCode.Trim() + item.MobilePhoneNumber.Trim();
                TwilioLib.SendSms(smsMessage, smsNumber);
                Console.WriteLine("Message Sent to: " + smsNumber + ", Message: " + smsMessage);
            }
        }

        [Verb(Aliases = "-s")]
        public static void Settings()
        {
            Db.DbConnectionString = Config.DatabaseConnString;
            Console.WriteLine("SmtpUsername: " + Config.SmtpUsername);
            Console.WriteLine("SmtpPassword: " + Config.SmtpPassword);
        }

        
    }
}
