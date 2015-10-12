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
        
        public static void SendSms(string message, string destinationNumber)
        {
            var client = new TwilioRestClient(AccountSid, AuthToken);
            client.SendMessage(FromNumber, destinationNumber, message);
        }
    }
}
