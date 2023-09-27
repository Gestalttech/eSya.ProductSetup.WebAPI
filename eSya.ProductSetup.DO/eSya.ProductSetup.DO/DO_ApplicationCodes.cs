using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.DO
{
    public class DO_ApplicationCodes
    {
        public int ApplicationCode { get; set; }
        public int CodeType { get; set; }
        public string CodeDesc { get; set; } = null!;
        public string? ShortCode { get; set; } = null!;
        public bool DefaultStatus { get; set; }
        public bool UsageStatus { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormID { get; set; } = null!;
        public int UserID { get; set; }
        public string TerminalID { get; set; } = null!;
    }
   
}
