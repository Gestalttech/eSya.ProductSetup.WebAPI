using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.DO
{
    public class DO_TaxStructure
    {
        public int ISDCode { get; set; }
        public int TaxCode { get; set; }
        public string TaxShortCode { get; set; } = null!;
        public string TaxDescription { get; set; } = null!;
        public string SlabOrPerc { get; set; } = null!;
        public bool IsSplitApplicable { get; set; }
        public bool SaveStatus { get; set; }
        public bool ActiveStatus { get; set; }
        public int UserID { get; set; }
        public string FormId { get; set; } = null!;
        public string TerminalID { get; set; } = null!;
    }
   
}
