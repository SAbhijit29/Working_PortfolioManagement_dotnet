using StockMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockMicroservice.Repository
{
   public interface IStockRepository
    {
        public StockDetails GetStockByNameRepository(string stockName);

        public List<StockDetails> GetAllStocks();
    }
}
