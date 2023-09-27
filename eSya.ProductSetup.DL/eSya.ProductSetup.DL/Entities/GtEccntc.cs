using System;
using System.Collections.Generic;

namespace eSya.ProductSetup.DL.Entities
{
    public partial class GtEccntc
    {
        public int Isdcode { get; set; }
        public int TaxCode { get; set; }
        public string TaxShortCode { get; set; } = null!;
        public string TaxDescription { get; set; } = null!;
        public string SlabOrPerc { get; set; } = null!;
        public bool IsSplitApplicable { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }

        public virtual GtEccncd IsdcodeNavigation { get; set; } = null!;
    }
}
