using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;

namespace BlueSMS
{
    public static class TwilioLib
    {
        public static string AccountSid { get; set; }
        public static string AuthToken { get; set; }
        public static string FromNumber { get; set; }

        public static Twilio.Message SendSms(string message, string destinationNumber)
        {
            var client = new TwilioRestClient(AccountSid, AuthToken);
            return client.SendMessage(FromNumber, destinationNumber, message);
        }

        public static Twilio.Message RetrieveMessage(string sid)
        {
            var client = new TwilioRestClient(AccountSid, AuthToken);
            return client.GetMessage(sid);
        }
    }   
}
