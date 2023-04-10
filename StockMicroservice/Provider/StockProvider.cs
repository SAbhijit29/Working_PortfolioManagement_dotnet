using StockMicroservice.Models;
using StockMicroservice.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockMicroservice.Provider
{
    public class StockProvider : IStockProvider
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(StockProvider));
        private readonly IStockRepository stockRepository;

        public StockProvider(IStockRepository _stockRepository)
        {
            stockRepository = _stockRepository;
        }

        public List<StockDetails> GetAllStocks()
        {
            List<StockDetails> s = new List<StockDetails>();
                s= stockRepository.GetAllStocks();
            return s;
        }

        public StockDetails GetStockByNameProvider(string stockName)
        {
            StockDetails stockData = null;

            try
            {
                _log4net.Info("StockController has Called GetStockByNameProvider and " + stockName + " is searched in StockProvider");
                stockData = stockRepository.GetStockByNameRepository(stockName);
            }
            catch (Exception e)
            {
                _log4net.Error("In StockProvider encountered exception" + e.Message);
            }
            return stockData;
        }
    }
}
