using System;
using System.Collections.Generic;

namespace eSya.ProductSetup.DL.Entities
{
    public partial class GtDccnpa
    {
        public int DocumentId { get; set; }
        public int ParameterId { get; set; }
        public bool ParamAction { get; set; }
        public bool ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }
    }
}
