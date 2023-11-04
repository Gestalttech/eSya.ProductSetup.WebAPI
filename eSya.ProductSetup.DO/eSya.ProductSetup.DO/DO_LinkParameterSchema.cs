using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.DO
{
    public class DO_LinkParameterSchema
    {
        public int LinkParameterType { get; set; }
        public int LinkParameterId { get; set; }
        public string SchemaId { get; set; } = null!;
        public bool ActiveStatus { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        public string? ParameterHeaderDesc { get; set; }
        public string? ParameterDesc { get; set; }
    }
}
