using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueSMS.Data
{
    public class SmsMessage
    {
        public Guid BatchGuid { get; set; }
        public string AgreementReference { get; set; }
        public string Sid { get; set; }
        public string Uri { get; set; }
        public string FromNumber { get; set; }
        public string ToNumber { get; set; }
        public string Body { get; set; }
        public string Status { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
