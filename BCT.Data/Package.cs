﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCT.Data
{
    public enum PackageType
    {
        Login
    }

    [Serializable]
    public class Package
    {
        public object Data { get; set; }

        public PackageType PackageType { get; set; }
    }
}
