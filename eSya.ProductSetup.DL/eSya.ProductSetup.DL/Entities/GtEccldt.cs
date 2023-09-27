using System;
using System.Collections.Generic;

namespace eSya.ProductSetup.DL.Entities
{
    public partial class GtEccldt
    {
        public int BusinessKey { get; set; }
        public decimal FinancialYear { get; set; }
        public int MonthId { get; set; }
        public bool MonthFreezeHis { get; set; }
        public bool MonthFreezeFin { get; set; }
        public bool MonthFreezeHr { get; set; }
        public int PatientIdgen { get; set; }
        public string PatientIdserial { get; set; } = null!;
        public string BudgetMonth { get; set; } = null!;
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }

        public virtual GtEcclco GtEcclco { get; set; } = null!;
    }
}
