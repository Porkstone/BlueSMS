﻿using CLAP;
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
            string reminderListHtml = "<img src='http://cdn.bluemotorfinance.co.uk/images/bluesms/blue-logo-mid-res-sm.jpg' /><p>SMS Payment Reminders will be sent to the following customers at 3pm tomorrow</p><table cellpadding='5px' style='width: 800px;'><thead style='background: lightgray;'><tr><th>Agreement</th><th style='width: 100px'>Customer Name</th><th>Bespoke Arrerars</th><th>Message</th></tr></thead><tbody>";
            var reminders = Db.GetPaymentRemindersReport();
            int counter = 0;
                foreach (var item in reminders)
                {
                    counter += 1;
                    string startTag = "<tr>";
                    if (IsEven(counter)) { startTag = "<tr style='background: #99ccff'"; }
                    reminderListHtml = reminderListHtml + startTag + "<td>" + item.AgreementReference + "</td><td>" + item.Forename + " " + item.Surname + "</td><td>" + item.BespokeArrearsState + "</td><td>" + SmsMessagePt1 + item.NextDueDate.ToString("d MMM yyyy") + SmsMessagePt2 + "</td></tr>";
                }
                reminderListHtml = reminderListHtml + "</tbody></table>";

            Smtp.SendHtmlEmail("SMS Payment Reminders Report", reminderListHtml);
	    }

        [Verb(Aliases = "-sendMessages")]
        public static void SendPaymentReminders()
        {
            const string SmsMessagePt1 = "Your next Blue Motor Finance loan repayment is due in 3 days on ";
            const string SmsMessagePt2 = ". Please ensure funds are available. If you have any issues please call 020 3005 9332";
            Console.WriteLine("Sending Payment Reminders. System Time: " + DateTime.Now.ToString("d MMM yyyy"));
            Db.DbConnectionString = Config.DatabaseConnString;
            TwilioLib.AccountSid = Config.TwilioAccountSid;
            TwilioLib.AuthToken = Config.TwilioAuthToken;
            TwilioLib.FromNumber = Config.TwilioFromNumber;

            var BatchGuid = Guid.NewGuid();
            string smsMessage = "";
            string smsNumber = "";
            var reminders = Db.GetPaymentReminders();
            foreach (var item in reminders)
            {
                smsMessage = SmsMessagePt1 + item.NextDueDate.ToString("d MMM yyyy") + SmsMessagePt2;
                smsNumber = item.MobilePhoneStdCode.Trim() + item.MobilePhoneNumber.Trim();

                // Override To number if we are not in the live environment
                if (Config.Environment != "LIVE") { smsNumber = "+44770928922"; }
                
                var msg = TwilioLib.SendSms(smsMessage, smsNumber);
                Console.WriteLine("Message Sent to: " + smsNumber + ", Message: " + smsMessage);
                Db.SaveOutboundMessage(BatchGuid, item.AgreementReference.Trim(), msg.Sid, msg.Uri.OriginalString, msg.From, msg.To, msg.Body, msg.Status);
            }
        }
        [Verb(Aliases = "-checkMessageStatuses")]
        public static void CheckMessageStatuses()
        {
            Db.DbConnectionString = Config.DatabaseConnString;

            var activeOutboundMessages = Db.GetActiveOutboundMessages();

            TwilioLib.AccountSid = Config.TwilioAccountSid;
            TwilioLib.AuthToken = Config.TwilioAuthToken;
            foreach (var item in activeOutboundMessages)
            {
                var msg = TwilioLib.RetrieveMessage(item.Sid);
                if (item.Status != msg.Status)
                {
                    string error = msg.ErrorCode.ToString() ?? "" + " : " +  msg.ErrorMessage ?? "";
                    Db.AppendToMessageLog(msg.Sid, error, msg.Status);
                 }       
            }
        }

        [Verb(Aliases = "-s")]
        public static void Settings()
        {
            Db.DbConnectionString = Config.DatabaseConnString;
            Console.WriteLine("SmtpUsername: " + Config.SmtpUsername);
            Console.WriteLine("SmtpPassword: " + Config.SmtpPassword);
        }

        private static bool IsEven(int number)
        {
            // Use modulus http://stackoverflow.com/questions/160930/how-do-i-check-if-an-integer-is-even-or-odd
            return (number % 2 == 0);
        }
    }
}
