using CLAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueSMS
{
    class Program
    {

        
        private string Encrypt = "";
        [STAThread]
        static void Main(string[] args)
        {
            const string SendToNumber = "+447770928922";
            const string Message = "Hello, This is your friendly neighbourhood finance company.";

            ParseCmdLineArgs(args);

            System.Threading.Thread.Sleep(5000);
            
            Console.WriteLine("Sending message: '" + Message + "'  to number: " + SendToNumber);
            //TwilioLib.SendSms(Message, SendToNumber);
        }

        private static void ParseCmdLineArgs(string[] args)
        {
            try
            {
                var myargs = Parser.Run<Main>(args); 
            }
            catch (Exception ex)
            {
                
                Console.WriteLine(ex.Message);
            }
            
            
        }
    }


}
