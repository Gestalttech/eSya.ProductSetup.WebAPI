using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.DO
{
    public class DO_CountryCodes
    {
        public int Isdcode { get; set; }
        public string CountryCode { get; set; } = null!;
        public string CountryName { get; set; } = null!;
        public string CountryFlag { get; set; } = null!;
        public string CurrencyCode { get; set; } = null!;
        public string MobileNumberPattern { get; set; } = null!;
        public int Nationality { get; set; }
        public bool IsPoboxApplicable { get; set; }
        public string? PoboxPattern { get; set; } = null!;
        public bool IsPinapplicable { get; set; }
        public string? PincodePattern { get; set; } = null!;
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int UserID { get; set; }
        public string TerminalID { get; set; } = null!;
        public string? CurrencyName { get; set; } = null!;
        public List<DO_UIDPattern>? _lstUIDpattern { get; set; } = null!;
        public string DateFormat { get; set; }
        public string ShortDateFormat { get; set; }

    }
    public class DO_UIDPattern
    {
        public int Isdcode { get; set; }
        public string Uidlabel { get; set; } = null!;
        public string Uidpattern { get; set; } = null!;
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int UserID { get; set; }
        public string TerminalID { get; set; } = null!;
    }
}
