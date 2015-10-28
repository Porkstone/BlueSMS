using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueSMS.Data
{
    public class SmsMessageStatus
    {
        public string Sid { get; set; }
        public string Status { get; set; }
        public string Error { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
