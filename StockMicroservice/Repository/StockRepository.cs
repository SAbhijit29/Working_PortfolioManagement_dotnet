using StockMicroservice.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockMicroservice.Repository
{
    public class StockRepository : IStockRepository
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(StockRepository));
        private static List<StockDetails> listOfStocks = new()
        {
            new StockDetails { StockId = 1, StockName = "TATA POWER", StockValue = 1500},
            new StockDetails { StockId = 2, StockName = "TRIDENT", StockValue = 55},
            new StockDetails { StockId = 3, StockName = "COAL INDIA", StockValue = 2600}
        };

        public List<StockDetails> GetAllStocks()
        {
            List<StockDetails> s = new List<StockDetails>();
            try
            {
                foreach (StockDetails item in listOfStocks)
                {
                    s.Add(item);
                }
            }
            catch (Exception e)
            {

                _log4net.Error("Exception encountered while fetching all stock details " + e.Message);
                return null;
            }
            return s;
        }

        public StockDetails GetStockByNameRepository(string stockName)
        {
            StockDetails stockData = null;
            try
            {
                string stockname = stockName.ToUpper();
                _log4net.Info("In StockRepository, StockProvider has Called GetStockByNameRepository and " + stockName + " is searched");
                stockData = listOfStocks.Where(e => e.StockName == stockName).FirstOrDefault();
                if (stockData != null)
                {
                    var jsonFund = JsonConvert.SerializeObject(stockData);
                    _log4net.Info("Stock Found " + jsonFund);
                }
                else
                {
                    _log4net.Info("Stock " + stockName + " not found in StockRepository");
                }
            }
            catch (Exception e)
            {
                _log4net.Error("Exception encountered while finding stock by name " + e.Message);
            }
            return stockData;
        }
    }
}
