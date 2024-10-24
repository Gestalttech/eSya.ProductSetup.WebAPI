﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.DO
{
    public class DO_SMSTEvent
    {
        public int TEventID { get; set; }
        public string TEventDesc { get; set; }
        public int MaxSequenceNumber { get; set; }
        public bool ActiveStatus { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        public string FormID { get; set; }
    }
    public class DO_SMSVariable
    {
        public string Smsvariable { get; set; }
        public string Smscomponent { get; set; }
        public bool ActiveStatus { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        public string FormID { get; set; }
    }
}
