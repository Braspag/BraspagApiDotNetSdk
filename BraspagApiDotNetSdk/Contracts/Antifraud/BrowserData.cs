﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BraspagApiDotNetSdk.Contracts.Antifraud
{
    public class BrowserData
    {
        public string HostName { get; set; }
        public bool CookiesAccepted { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
        public string IpAddress { get; set; }
    }
}
