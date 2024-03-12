using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.DO
{
    public class DO_ProcessRule
    {
        public int RuleId { get; set; }
        public int ProcessId { get; set; }
        public string RuleDesc { get; set; }
        public string? Notes { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
    }
   
}
