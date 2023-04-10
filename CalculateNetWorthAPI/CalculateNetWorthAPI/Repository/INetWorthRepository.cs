using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalculateNetWorthAPI.Models;
namespace CalculateNetWorthAPI.Repository
{
    public interface INetWorthRepository
    {
        public Task<NetWorth> calculateNetWorthAsync(PortFolioDetails portFolioDetails);

        public AssetSaleResponse sellAssets(PortFolioDetails currentHoldingAndToSell);

        public PortFolioDetails GetPortFolioDetailsByID(int id);

        public List<PortFolioDetails> GetAllPortfolioPersonsDetails();
    }
}
