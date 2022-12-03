﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeller.Models
{
    public class PropellerUser
    {
        public string Name { get; set; } = string.Empty;
        public int Role { get; set; }
        public string Locale { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;

    }
}
