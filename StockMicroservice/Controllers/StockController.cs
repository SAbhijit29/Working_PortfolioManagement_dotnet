using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockMicroservice.Models;
using StockMicroservice.Provider;

namespace StockMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(StockController));
        private readonly IStockProvider stockProvider;

        public StockController(IStockProvider _stockProvider)
        {
            _log4net.Info("StockController Called");
            stockProvider = _stockProvider;
        }

        [HttpGet("")]
        public List<StockDetails> GetAllStocks()
        {
            List<StockDetails> list =  stockProvider.GetAllStocks();
            return list;
        }

        [HttpGet("{stockName}")]
        public IActionResult GetStocksDetailsByName(string stockName)
        {
            var stockData = stockProvider.GetStockByNameProvider(stockName);
            try
            {
                if(string.IsNullOrEmpty(stockName))
                {
                    _log4net.Info("Stock Name is Null");
                    return BadRequest("Name cannot be null");
                }
                _log4net.Info("In Stock Controller " + stockName + " is found with....");
                if(stockData == null)
                {
                    _log4net.Info(stockName + " is invalid Stock.");
                    return NotFound("Invalid Stock Name");
                }
                else
                {
                    _log4net.Info(stockName + " stock Found.");
                    return Ok(stockData);
                }
            }
            catch (Exception e)
            {
                _log4net.Error("Exception Found = " + e.Message);
                return new StatusCodeResult(500);
            }
        }
    }
}
