using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.DO
{
    public class DO_Subledger
    {
        public string SubledgerType { get; set; } 
        public string? Sltdesc { get; set; }
        public int SubledgerGroup { get; set; }
        public string? SubledgerDesc { get; set; } 
        public string? Coahead { get; set; }
        public bool ActiveStatus { get;set; }
        public int UserID { get; set; }
        public string FormID { get; set; }
        public string TerminalID { get; set; }
    }
}
