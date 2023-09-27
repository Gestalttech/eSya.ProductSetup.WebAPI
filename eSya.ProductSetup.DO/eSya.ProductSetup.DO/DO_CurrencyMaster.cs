using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.DO
{
    public class DO_CurrencyMaster
    {
        public string CurrencyCode { get; set; } = null!;
        public string CurrencyName { get; set; } = null!;
        public string Symbol { get; set; } = null!;
        public decimal DecimalPlaces { get; set; }
        public bool ShowInMillions { get; set; }
        public bool SymbolSuffixToAmount { get; set; }
        public string DecimalPortionWord { get; set; } = null!;
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int UserID { get; set; }
        public string TerminalID { get; set; } = null!;
    }
    public class DO_ExchangeRate
    {
        public int BusinessKey { get; set; }
        public string CurrencyCode { get; set; } = null!;
        public DateTime DateOfExchangeRate { get; set; }
        public decimal? StandardRate { get; set; }
        public decimal? SellingRate { get; set; }
        public DateTime? SellingLastVoucherDate { get; set; }
        public decimal? BuyingRate { get; set; }
        public DateTime? BuyingLastVoucherDate { get; set; }
        public bool ActiveStatus { get; set; }
        public string CurrencyName { get; set; } = null!;
        public int UserID { get; set; }
        public string TerminalID { get; set; } = null!;
    }
}
