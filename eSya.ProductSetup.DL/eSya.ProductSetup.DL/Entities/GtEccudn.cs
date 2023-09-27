using System;
using System.Collections.Generic;

namespace eSya.ProductSetup.DL.Entities
{
    public partial class GtEccudn
    {
        public string CurrencyCode { get; set; } = null!;
        public string BnorCnId { get; set; } = null!;
        public decimal DenomId { get; set; }
        public string DenomDesc { get; set; } = null!;
        public decimal DenomConversion { get; set; }
        public int Sequence { get; set; }
        public DateTime EffectiveDate { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }

        public virtual GtEccuco CurrencyCodeNavigation { get; set; } = null!;
    }
}
