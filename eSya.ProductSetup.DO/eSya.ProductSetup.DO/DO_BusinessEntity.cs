using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.DO
{
    public class DO_BusinessEntity
    {
        public int BusinessId { get; set; }
        public string BusinessDesc { get; set; }
        public bool IsMultiSegmentApplicable { get; set; }
        public string BusinessUnitType { get; set; }
        public int NoOfUnits { get; set; }
        public int ActiveNoOfUnits { get; set; }
        public bool UsageStatus { get; set; }
        public bool ActiveStatus { get; set; }
        //public string FormId { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
    }
    public class DO_BusinessSubscription
    {
        public int BusinessKey { get; set; }
        public string? LocationDescription { get; set; }
        public DateTime SubscribedFrom { get; set; }
        public DateTime SubscribedTill { get; set; }
        public bool ActiveStatus { get; set; }
        public int UserID { get; set; }
        public string FormID { get; set; }
        public string TerminalID { get; set; }
        public int isEdit { get; set; }
    }
    public class DO_TaxIdentification
    {
        public int Isdcode { get; set; }
        public int TaxCode { get; set; }
        public string? TaxDesc { get; set; }
        public int TaxIdentificationId { get; set; }
        public string TaxIdentificationDesc { get; set; }
        public string StateCode { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
    }

}
