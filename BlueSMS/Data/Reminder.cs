using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueSMS.Data
{
    public class Reminder
    {
        public string AgreementReference { get; set; }
        public DateTime NextDueDate { get; set; }
        public decimal ArrearsState { get; set; }
        public decimal MiArrearsState { get; set; }
        public decimal BespokeArrearsState { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string MobilePhoneStdCode { get; set; }
        public string MobilePhoneNumber { get; set; }
    }
}
