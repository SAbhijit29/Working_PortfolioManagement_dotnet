using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalculateNetWorthAPI.Models;

namespace CalculateNetWorthAPI.Provider
{
    public interface INetWorthProvider
    {
        public Task<NetWorth> calculateNetWorthAsync(PortFolioDetails portFolioDetails);

        public AssetSaleResponse sellAssets(PortFolioDetails currentHoldingsAndToSell);

        public PortFolioDetails GetPortFolioDetailsByID(int id);

        public List<PortFolioDetails> GetAllPortfolioPersonsDetails();
    }
}
