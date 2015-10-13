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
             ParseCmdLineArgs(args);
            System.Threading.Thread.Sleep(5000);
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
