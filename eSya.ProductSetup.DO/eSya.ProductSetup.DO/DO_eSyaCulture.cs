using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.DO
{
    public class DO_eSyaCulture
    {
        public string CultureCode { get; set; } 
        public string CultureDesc { get; set; } 
        public bool ActiveStatus { get; set; }
        public string FormID { get; set; } 
        public int UserID { get; set; }
        public string TerminalID { get; set; } 
    }
}
