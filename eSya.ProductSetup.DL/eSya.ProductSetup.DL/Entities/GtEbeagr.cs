﻿using System;
using System.Collections.Generic;

namespace eSya.ProductSetup.DL.Entities
{
    public partial class GtEbeagr
    {
        public int AgeRangeId { get; set; }
        public string RangeDesc { get; set; } = null!;
        public int AgeRangeFrom { get; set; }
        public string RangeFromPeriod { get; set; } = null!;
        public int AgeRangeTo { get; set; }
        public string RangeToPeriod { get; set; } = null!;
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }
    }
}
