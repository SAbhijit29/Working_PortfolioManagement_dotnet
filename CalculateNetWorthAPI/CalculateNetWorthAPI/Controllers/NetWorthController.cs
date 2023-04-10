using CalculateNetWorthAPI.Provider;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CalculateNetWorthAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateNetWorthAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NetWorthController : ControllerBase
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(NetWorthController));

        private readonly INetWorthProvider _netWorthProvider;


        public NetWorthController(INetWorthProvider netWorthProvider)
        {
            _netWorthProvider = netWorthProvider;
        }

        [HttpGet("")]
        public List<PortFolioDetails> GetPortFolioDetailsAll()
        {
            List<PortFolioDetails> pfs = _netWorthProvider.GetAllPortfolioPersonsDetails();
            return pfs;
        }

        [HttpGet("{portFolioId}")]
        public IActionResult GetPortFolioDetailsByID(int portFolioId)
        {
            PortFolioDetails portFolioDetails = new PortFolioDetails();
            try
            {
                if (portFolioId <= 0)
                {
                    return NotFound("ID can't be 0 or less than 0");
                }
                portFolioDetails = _netWorthProvider.GetPortFolioDetailsByID(portFolioId);
                if (portFolioDetails == null)
                {
                    return NotFound("Sorry, We don't have a portfolio with that ID");
                }
                else
                {
                    _log4net.Info("The deatils of the user are" + JsonConvert.SerializeObject(portFolioDetails));
                    return Ok(portFolioDetails);
                }
            }
            catch (Exception ex)
            {
                _log4net.Info("An exception occured while fetching the portfolio by id in the " + nameof(GetPortFolioDetailsByID) + ". The message is:" + ex.Message);
                return new StatusCodeResult(500);
            }
        }

        [HttpPost]
        public IActionResult GetNetWorth(PortFolioDetails portFolioDetails)
        {

            NetWorth _netWorth = new NetWorth();
            _log4net.Info("Calculating the networth of user with id = " + portFolioDetails.PortFolioId + "In the method:" + nameof(GetNetWorth));

            try
            {
                if (portFolioDetails == null)
                {
                    return NotFound("The portfolio doesn't contain any data");
                }
                else if (portFolioDetails.PortFolioId == 0)
                {
                    return NotFound("The user with that id not found");
                }
                else
                {
                    _log4net.Info("The portfolio details are correct.Returning the networth of user with id" + portFolioDetails.PortFolioId);
                    _netWorth = _netWorthProvider.calculateNetWorthAsync(portFolioDetails).Result;
                    _log4net.Info("The networth is:" + JsonConvert.SerializeObject(_netWorth));
                    return Ok(_netWorth);
                }
            }
            catch (Exception ex)
            {
                _log4net.Info("An exception occured while calculating the networth:" + ex.Message + " In the controller" + nameof(GetNetWorth));
                return new StatusCodeResult(500);
            }
        }

        [HttpPost]
        public IActionResult SellAssets(PortFolioDetails listOfAssetsCurrentlyHoldingAndAssetsToSell)
        {
            try
            {
                AssetSaleResponse assetSaleResponse = new AssetSaleResponse();
                if (listOfAssetsCurrentlyHoldingAndAssetsToSell == null || listOfAssetsCurrentlyHoldingAndAssetsToSell == null)
                {
                    _log4net.Info("The portfolio provided don't hold any data");
                    return NotFound("Please Provide a Valid List of portFolios");
                }
                else
                {

                   // foreach (PortFolioDetails portFolioDetails in listOfAssetsCurrentlyHoldingAndAssetsToSell)
                    {
                        if ( listOfAssetsCurrentlyHoldingAndAssetsToSell.PortFolioId == 0)
                        {
                            return NotFound("We didn't find any portfolio with that Id");
                        }
                    }
                    assetSaleResponse = _netWorthProvider.sellAssets(listOfAssetsCurrentlyHoldingAndAssetsToSell);
                    if (assetSaleResponse == null)
                    {
                        _log4net.Info("Couldn't be sold because of invalid portfolio");
                        return NotFound("Please provide a valid list of portfolios");
                    }
                    _log4net.Info("The response is" + JsonConvert.SerializeObject(assetSaleResponse));
                    return Ok(assetSaleResponse);
                }
            }
            catch (Exception ex)
            {
                _log4net.Info("An exception occured while calculating the networth:" + ex + " In the controller" + nameof(SellAssets));
                return new StatusCodeResult(500);
            }
        }


    }

}
