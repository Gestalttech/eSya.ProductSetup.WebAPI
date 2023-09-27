using System;
using System.Collections.Generic;

namespace eSya.ProductSetup.DL.Entities
{
    public partial class GtEcclco
    {
        public GtEcclco()
        {
            GtEccldts = new HashSet<GtEccldt>();
        }

        public int BusinessKey { get; set; }
        public decimal FinancialYear { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime TillDate { get; set; }
        public bool Status { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }

        public virtual GtEcbsln BusinessKeyNavigation { get; set; } = null!;
        public virtual ICollection<GtEccldt> GtEccldts { get; set; }
    }
}
