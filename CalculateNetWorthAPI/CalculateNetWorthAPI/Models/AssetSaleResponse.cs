﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateNetWorthAPI.Models
{
    public class AssetSaleResponse
    {
        public bool SaleStatus { get; set; }

        public double Networth { get; set; }

    }
}
