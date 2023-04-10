using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyMutualFundNAVMicroservice.Models;
using DailyMutualFundNAVMicroservice.Provider;

namespace DailyMutualFundNAVMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MutualFundNAVController : ControllerBase
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(MutualFundNAVController));
        private readonly IMutualFundProvider mutualFundProvider;

        public MutualFundNAVController(IMutualFundProvider _mutualFundProvider)
        {
            _log4net.Info("MutualFundNAVController Called");
            mutualFundProvider = _mutualFundProvider;
        }

        [HttpGet("")]
        public List<MutualFundDetails> GetAllMutual()
        {
            List<MutualFundDetails> list = mutualFundProvider.GetAllMutual();
            return list;
        }


        [HttpGet("{mutualFundName}")]
        public IActionResult GetMutualFundDetailsByName(string mutualFundName)
        {
            var mutualFundData = mutualFundProvider.GetMutualFundByNameProvider(mutualFundName);
            try
            {
                if(string.IsNullOrEmpty(mutualFundName))
                {
                    _log4net.Info("MutualFund Name is Null");
                    return BadRequest("Name cannot be null");
                }
                _log4net.Info("In MutualFundNAV Controller " + mutualFundName + " is found with....");
                if(mutualFundData == null)
                {
                    _log4net.Info(mutualFundName + " is invalid MutualFund.");
                    return NotFound("Invalid MutualFund Name");
                }
                else
                {
                    _log4net.Info(mutualFundName + " MutualFund Found.");
                    return Ok(mutualFundData);
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
