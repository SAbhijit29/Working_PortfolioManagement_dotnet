using CalculateNetWorthAPI.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace CalculateNetWorthAPI.Repository
{
    public class NetWorthRepository : INetWorthRepository
    {

        private IConfiguration configuration;

        public NetWorthRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        private readonly static log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(NetWorthRepository));

        public static List<PortFolioDetails> _portFolioDetails = new List<PortFolioDetails>()
        {
            new PortFolioDetails{
                    PortFolioId=777,
                    MutualFundList = new List<MutualFundDetails>()
                    {
                        new MutualFundDetails{MutualFundName = "SBI", MutualFundUnits=44},
                        new MutualFundDetails{MutualFundName = "KOTAK", MutualFundUnits=66}
                    },
                    StockList = new List<StockDetails>()
                    {
                        new StockDetails{StockCount = 190, StockName = "TATA POWER"},
                        new StockDetails{StockCount = 667, StockName = "TRIDENT"}
                    }
                },
             new PortFolioDetails
                {
                    PortFolioId = 888,
                    MutualFundList = new List<MutualFundDetails>()
                    {
                        new MutualFundDetails{MutualFundName = "SBI", MutualFundUnits=340},
                        new MutualFundDetails{MutualFundName = "KOTAK", MutualFundUnits=100}
                    },
                    StockList = new List<StockDetails>()
                    {
                        new StockDetails{StockCount = 240, StockName = "TATA POWER"},
                        new StockDetails{StockCount = 460, StockName = "TRIDENT"}
                    }
                },
                new PortFolioDetails
                {
                    PortFolioId = 999,
                    MutualFundList = new List<MutualFundDetails>()
                    {
                        new MutualFundDetails{MutualFundName = "SBI", MutualFundUnits=800},
                        new MutualFundDetails{MutualFundName = "KOTAK", MutualFundUnits=600},
                        new MutualFundDetails{MutualFundName = "RELIANCE", MutualFundUnits=600}
                    },
                    StockList = new List<StockDetails>()
                    {
                        new StockDetails{StockCount = 200, StockName = "TATA POWER"},
                        new StockDetails{StockCount = 342, StockName = "TRIDENT"},
                        new StockDetails{StockCount = 122, StockName = "COAL INDIA"}
                    }
                }
        };
        public async Task<NetWorth> calculateNetWorthAsync(PortFolioDetails portFolioDetails)
        {

            Stock stock = new Stock();
            MutualFund mutualfund = new MutualFund();
            NetWorth networth = new NetWorth();
            _log4net.Info("Calculating the networth in the repository method of user with id = " + portFolioDetails.PortFolioId);
            try
            {
                using (var httpClient = new HttpClient())
                {

                    var fetchStock = configuration["GetStockDetails"];
                    var fetchMutualFund = configuration["GetMutualFundDetails"];
                    if (portFolioDetails.StockList != null && portFolioDetails.StockList.Any() == true)
                    {
                        foreach (StockDetails stockDetails in portFolioDetails.StockList)
                        {
                            if (stockDetails.StockName != null)
                            {
                                using (var response = await httpClient.GetAsync(fetchStock + stockDetails.StockName))
                                {
                                    _log4net.Info("Fetching the details of stock " + stockDetails.StockName + "from the stock api");
                                    string apiResponse = await response.Content.ReadAsStringAsync();
                                    stock = JsonConvert.DeserializeObject<Stock>(apiResponse);
                                    _log4net.Info("Th stock details are " + JsonConvert.SerializeObject(stock));
                                }
                                networth.Networth += stockDetails.StockCount * stock.StockValue;
                            }
                        }
                    }
                    if (portFolioDetails.MutualFundList != null && portFolioDetails.MutualFundList.Any() == true)
                    {
                        foreach (MutualFundDetails mutualFundDetails in portFolioDetails.MutualFundList)
                        {
                            if (mutualFundDetails.MutualFundName != null)
                            {
                                using (var response = await httpClient.GetAsync(fetchMutualFund + mutualFundDetails.MutualFundName))
                                {
                                    _log4net.Info("Fetching the details of mutual Fund " + mutualFundDetails.MutualFundName + "from the MutualFundNAV api");
                                    string apiResponse = await response.Content.ReadAsStringAsync();
                                    mutualfund = JsonConvert.DeserializeObject<MutualFund>(apiResponse);
                                    _log4net.Info("The mutual Fund Details are" + JsonConvert.SerializeObject(mutualfund));
                                }
                                networth.Networth += mutualFundDetails.MutualFundUnits * mutualfund.MutualFundValue;
                            }
                        }
                    }
                }
                networth.Networth = Math.Round(networth.Networth, 2);
            }
            catch (Exception ex)
            {
                _log4net.Error("Exception occured while calculating the networth of user" + portFolioDetails.PortFolioId + ":" + ex.Message);
            }
            return networth;
        }



        public async Task<NetWorth> calculateNetWorth(PortFolioDetails portFolio)
        {
            try
            {
                NetWorth _networth = new NetWorth();

                Stock stock = new Stock();
                MutualFund mutualFund = new MutualFund();
                double networth = 0;

                using (var httpClient = new HttpClient())
                {

                    var fetchStock = configuration["GetStockDetails"];
                    var fetchMutualFund = configuration["GetMutualFundDetails"];
                    if (portFolio.StockList != null || portFolio.MutualFundList.Any() != false)
                    {
                        foreach (StockDetails stockDetails in portFolio.StockList)
                        {
                            using (var response = await httpClient.GetAsync(fetchStock + stockDetails.StockName))
                            {
                                _log4net.Info("Fetching the details of stock " + stockDetails.StockName + " from the stock api");
                                string apiResponse = await response.Content.ReadAsStringAsync();
                                stock = JsonConvert.DeserializeObject<Stock>(apiResponse);
                            }
                            networth += stockDetails.StockCount * stock.StockValue;
                        }
                    }
                    if (portFolio.MutualFundList != null || portFolio.MutualFundList.Any() != false)
                    {
                        foreach (MutualFundDetails mutualFundDetails in portFolio.MutualFundList)
                        {
                            using (var response = await httpClient.GetAsync(fetchMutualFund + mutualFundDetails.MutualFundName))
                            {
                                _log4net.Info("Fetching the details of stock " + mutualFundDetails.MutualFundName + " from the stock api");
                                string apiResponse = await response.Content.ReadAsStringAsync();
                                mutualFund = JsonConvert.DeserializeObject<MutualFund>(apiResponse);
                            }
                            networth += mutualFundDetails.MutualFundUnits * mutualFund.MutualFundValue;
                        }
                    }
                }
                networth = Math.Round(networth, 2);
                _networth.Networth = networth;
                return _networth;

            }
            catch (Exception ex)
            {
                _log4net.Error("An exception occured while selling the assets:" + ex.Message);
                return null;
            }
        }

        public AssetSaleResponse sellAssets(PortFolioDetails details)
        {
            //details[0].PortFolioId;
            NetWorth networth = new NetWorth();
            NetWorth networth2 = new NetWorth();

            int flag1 = 1;
            int flag2 = 1;
            StockDetails stocktoremove = new StockDetails();
            MutualFundDetails fundToRemove = new MutualFundDetails();

            AssetSaleResponse assetSaleResponse = new AssetSaleResponse();
            PortFolioDetails current = GetPortFolioDetailsByID(details.PortFolioId); 
            PortFolioDetails toSell = details; //portfoliodetailschandra tosell 
            try
            {

                //foreach (PortFolioDetails portFolioDetails in details)
                {
                    if (details == null)
                    {
                        return null;
                    }
                }

                //_log4net.Info("Selling the assets of user with id" + details[0].PortFolioId);
                networth = calculateNetWorth(current).Result;  //returning total current net worth before selling
                assetSaleResponse.SaleStatus = true;


                foreach (StockDetails toSellStock in toSell.StockList)
                {
                    foreach (StockDetails currentStock in current.StockList)
                    {
                        if (currentStock.StockName == toSellStock.StockName)
                        {
                            if (currentStock.StockCount < toSellStock.StockCount)
                            {
                                _log4net.Info("Not enough stocks to sell for user :" + current.PortFolioId);
                                assetSaleResponse.SaleStatus = false;
                                assetSaleResponse.Networth = networth.Networth;
                                return assetSaleResponse;
                            }
                            break;
                        }

                    }
                }


                foreach (MutualFundDetails toSellMutualFund in toSell.MutualFundList)
                {
                    foreach (MutualFundDetails currentMutualFund in current.MutualFundList)
                    {
                        if (currentMutualFund.MutualFundName == toSellMutualFund.MutualFundName)
                        {
                            if (currentMutualFund.MutualFundUnits < toSellMutualFund.MutualFundUnits)
                            {
                                _log4net.Info("Not enough mutualFunds to sell for user" + current.PortFolioId);
                                assetSaleResponse.SaleStatus = false;
                                assetSaleResponse.Networth = networth.Networth;
                                return assetSaleResponse;
                            }
                            break;
                        }

                    }
                }




                foreach (PortFolioDetails portfolio in _portFolioDetails)
                {
                    if (portfolio.PortFolioId == toSell.PortFolioId)
                    {
                        foreach (StockDetails currentstock in portfolio.StockList)
                        {
                            foreach (StockDetails sellstock in toSell.StockList)
                            {
                                if (sellstock.StockName == currentstock.StockName)
                                {
                                    currentstock.StockCount = currentstock.StockCount - sellstock.StockCount;
                                    if (currentstock.StockCount == 0)
                                    {
                                        _log4net.Info("The user with id = " + current.PortFolioId + "sold all of his " + currentstock.StockName + " stocks.");
                                        flag1 = 0;
                                        stocktoremove = currentstock;

                                        break;
                                    }

                                }

                            }

                        }


                        foreach (MutualFundDetails currentmutualfund in portfolio.MutualFundList)
                        {
                            foreach (MutualFundDetails sellmutualfund in toSell.MutualFundList)
                            {
                                if (sellmutualfund.MutualFundName == currentmutualfund.MutualFundName)
                                {
                                    currentmutualfund.MutualFundUnits = currentmutualfund.MutualFundUnits - sellmutualfund.MutualFundUnits;
                                    if (currentmutualfund.MutualFundUnits == 0)
                                    {
                                        _log4net.Info("The user with id = " + current.PortFolioId + " has sold all of his mutual funds of" + currentmutualfund.MutualFundName);
                                        flag2 = 0;
                                        fundToRemove = currentmutualfund;

                                        break;
                                    }

                                }

                            }
                        }
                    }
                }
                if (flag1 == 0)
                {
                    foreach (PortFolioDetails portfolio in _portFolioDetails)
                    {
                        if (portfolio.PortFolioId == current.PortFolioId)
                        {
                            portfolio.StockList.Remove(stocktoremove);
                        }
                    }
                }
                if (flag2 == 0)
                {
                    foreach (PortFolioDetails portfolio in _portFolioDetails)
                    {
                        if (portfolio.PortFolioId == current.PortFolioId)
                        {
                            portfolio.MutualFundList.Remove(fundToRemove);
                        }
                    }
                }

                networth2 = calculateNetWorth(toSell).Result;
                assetSaleResponse.Networth = networth.Networth - networth2.Networth;
                _log4net.Info("The sale ststus of user with id = " + current.PortFolioId + " is equal to" + JsonConvert.SerializeObject(assetSaleResponse));
                //
            }
            catch (Exception ex)
            {
                _log4net.Error("An exception occured while selling the assets:" + ex.Message);
            }
            return assetSaleResponse;
        }

        public PortFolioDetails GetPortFolioDetailsByID(int id)
        {
            PortFolioDetails portFolioDetails = new PortFolioDetails();
            try
            {
                _log4net.Info("Returning portfolio details with the id" + id + " from the repository method");
                portFolioDetails = _portFolioDetails.FirstOrDefault(e => e.PortFolioId == id);
            }
            catch (Exception ex)
            {
                _log4net.Error("An exception occured while fetching the portFolio details:" + ex.Message);
            }
            return portFolioDetails;
        }

        public List<PortFolioDetails> GetAllPortfolioPersonsDetails()
        {
            List<PortFolioDetails> pfs = new List<PortFolioDetails>();
            
            foreach (PortFolioDetails item in _portFolioDetails)
            {
                pfs.Add((PortFolioDetails)item);
            }
            return pfs;
        }

    }
}
