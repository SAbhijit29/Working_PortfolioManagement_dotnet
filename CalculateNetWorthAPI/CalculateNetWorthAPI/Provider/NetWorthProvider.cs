using CalculateNetWorthAPI.Models;
using CalculateNetWorthAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateNetWorthAPI.Provider
{
    public class NetWorthProvider : INetWorthProvider
    {
        private readonly INetWorthRepository _netWorthRepository;
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(NetWorthProvider));


        public NetWorthProvider(INetWorthRepository netWorthRepository)
        {
            _netWorthRepository = netWorthRepository;
        }
        public Task<NetWorth> calculateNetWorthAsync(PortFolioDetails portFolioDetails)
        {
            NetWorth networth = new NetWorth();
            try
            {
                _log4net.Info("Provider called from Controller to calculate the networth");
                if (portFolioDetails.PortFolioId == 0)
                {
                    return null;
                }
                networth = _netWorthRepository.calculateNetWorthAsync(portFolioDetails).Result;
            }
            catch (Exception ex)
            {
                _log4net.Error("Exception occured while calculating the networth" + ex.Message);
            }
            return Task.FromResult(networth);


        }

        public PortFolioDetails GetPortFolioDetailsByID(int id)
        {
            PortFolioDetails portfolioDetails = new PortFolioDetails();
            try
            {
                _log4net.Info("Returning the portfolio object from the provider method of user :" + id);
                portfolioDetails = _netWorthRepository.GetPortFolioDetailsByID(id);
            }
            catch (Exception ex)
            {
                _log4net.Error("Exception ocured while getting the portfolio:" + ex.Message);
            }
            return portfolioDetails;
        }

        public AssetSaleResponse sellAssets(PortFolioDetails currentHoldingsAndToSell)
        {
            AssetSaleResponse assetSaleResponse = new AssetSaleResponse();
            try
            {


                //if (currentHoldingsAndToSell.Any() == false)
                if(currentHoldingsAndToSell==null)
                {
                    return null;
                }
                //_log4net.Info(nameof(sellAssets) + " method called to sell assets of user with id = " + currentHoldingsAndToSell[0].PortFolioId);
                assetSaleResponse = _netWorthRepository.sellAssets(currentHoldingsAndToSell);

            }
            catch (Exception ex)
            {
                _log4net.Error("Exception occured while selling the assets:" + ex.Message);
            }
            return assetSaleResponse;
        }

        public List<PortFolioDetails> GetAllPortfolioPersonsDetails()
        {
            List<PortFolioDetails> pfs = 
                _netWorthRepository.GetAllPortfolioPersonsDetails();
            return pfs;
        }
    }
}
