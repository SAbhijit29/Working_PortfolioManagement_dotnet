using StockMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockMicroservice.Provider
{
    public interface IStockProvider
    {
        public StockDetails GetStockByNameProvider(string stockName);
        public List<StockDetails> GetAllStocks();
    }
}
