using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateNetWorthAPI.Models
{
    public class PortFolioDetails
    {
        public int PortFolioId { get; set; } //777

        public List<StockDetails> StockList { get; set; } // {tcs, 10}

        public List<MutualFundDetails> MutualFundList { get; set; } // 


    }

}
