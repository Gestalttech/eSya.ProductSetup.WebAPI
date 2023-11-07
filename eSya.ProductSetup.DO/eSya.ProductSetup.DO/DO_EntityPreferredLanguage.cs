﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.DO
{
    public class DO_EntityPreferredLanguage
    {
        public int BusinessId { get; set; }
        //public string PreferredLanguage { get; set; } = null!;
        public string CultureCode { get; set; }
        public string? CultureDesc { get; set; } 
        public string Pldesc { get; set; }
        public bool DefaultLanguage { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }


    }
}
