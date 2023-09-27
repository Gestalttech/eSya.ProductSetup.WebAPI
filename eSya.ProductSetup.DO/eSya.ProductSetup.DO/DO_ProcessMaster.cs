﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.DO
{
    public class DO_ProcessMaster
    {
        public int ProcessId { get; set; }
        public string ProcessDesc { get; set; }
        public bool SystemControl { get; set; }
        public bool IsSegmentSpecific { get; set; }
        public string ProcessControl { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
    }
}
