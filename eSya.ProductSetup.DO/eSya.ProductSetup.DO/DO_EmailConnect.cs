using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.DO
{
    public class DO_EmailConnect
    {
        public int BusinessKey { get; set; }
        public string OutgoingMailServer { get; set; } 
        public int Port { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormID { get; set; } 
        public int UserID { get; set; }
        public string TerminalID { get; set; } 
        public int ISDCode { get; set; }
        public bool status { get; set; }
    }
}
