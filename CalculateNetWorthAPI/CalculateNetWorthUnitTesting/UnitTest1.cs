using NUnit.Framework;
using Moq;
using CalculateNetWorthAPI.Controllers;
using CalculateNetWorthAPI.Models;
using CalculateNetWorthAPI.Provider;
using CalculateNetWorthAPI.Repository;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace CalculateNetWorthUnitTesting
{
    [TestFixture]
    public class Tests
    {
        public NetWorthProvider netWorthProvider;
        private readonly Mock<INetWorthRepository> netWorthRepository = new Mock<INetWorthRepository>();
        private readonly Mock<INetWorthProvider> networthProviderMock = new Mock<INetWorthProvider>();
        public double networth;
        NetWorth netWorth;
        public PortFolioDetails _portFolioDetails;
        public NetWorthController netWorthController;
        public AssetSaleResponse assetSaleResponse;

        public Tests()
        {
            netWorthProvider = new NetWorthProvider(netWorthRepository.Object);
            netWorthController = new NetWorthController(networthProviderMock.Object);
        }
        [SetUp]
        public void Setup()
        {
            networth = 10789.97;
            netWorth = new NetWorth();
            netWorth.Networth = networth;
            assetSaleResponse = new AssetSaleResponse() { SaleStatus = true, Networth = 12456.44 };
            _portFolioDetails = new PortFolioDetails()
            {
                PortFolioId = 123,
                MutualFundList = new List<MutualFundDetails>()
                {
                    new MutualFundDetails{MutualFundName = "Cred", MutualFundUnits=3},
                        new MutualFundDetails{MutualFundName = "Viva", MutualFundUnits=5}
                },
                StockList = new List<StockDetails>()
                    {
                        new StockDetails{StockCount = 1, StockName = "BTC"},
                        new StockDetails{StockCount = 6, StockName = "ETH"}
                    }
            };
        }

        [Test]
        public void TestForCalculatingNetWorthWhenObjectHasValuesinRepository()
        {
            netWorthRepository.Setup(c => c.calculateNetWorthAsync(_portFolioDetails)).ReturnsAsync(netWorth);
            var res = netWorthProvider.calculateNetWorthAsync(_portFolioDetails).Result;
            Assert.AreEqual(res.Networth, networth);
            Assert.Pass();
        }

        [Test]
        public void TestForCalculatingNetWorthWhenObjectisNullinRepository()
        {
            netWorthRepository.Setup(x => x.calculateNetWorthAsync(It.Ref<PortFolioDetails>.IsAny)).ReturnsAsync(() => null);
            PortFolioDetails portfolio = new PortFolioDetails();
            var result = netWorthProvider.calculateNetWorthAsync(portfolio);
            Assert.IsNull(result);
            Assert.Pass();
        }

        [Test]
        public void TestForResponseWhenObjectHaveValuesinRepository()
        {
            netWorthRepository.Setup(x => x.sellAssets(_portFolioDetails)).Returns(assetSaleResponse);
            var result = netWorthProvider.sellAssets(_portFolioDetails);
            Assert.AreEqual(result.Networth, assetSaleResponse.Networth);
        }

        [Test]
        public void TestForResponseWhenEitherObjectIsNullinRepository()
        {
            netWorthRepository.Setup(x => x.sellAssets(It.Ref<PortFolioDetails>.IsAny)).Returns(() => null);
            PortFolioDetails portfoliodetails = new PortFolioDetails();
            var result = netWorthProvider.sellAssets(portfoliodetails);
            Assert.IsNull(result);

        }


        //testing for provider starts here.
        [Test]
        public void TestForCalculatingNetWorthWhenObjectHasValuesinProvider()
        {
            networthProviderMock.Setup(x => x.calculateNetWorthAsync(_portFolioDetails)).ReturnsAsync(new NetWorth { Networth = networth });
            var data = netWorthController.GetNetWorth(_portFolioDetails);
            ObjectResult okResult = data as ObjectResult;

            Assert.AreEqual(200, okResult.StatusCode);
            Assert.Pass();

        }

        [Test]
        public void TestForCalculatingNetWorthWhenObjectDoesNotHaveValuesinProvider()
        {
            PortFolioDetails portFolioDetails = new PortFolioDetails();
            networthProviderMock.Setup(x => x.calculateNetWorthAsync(It.Ref<PortFolioDetails>.IsAny)).ReturnsAsync(() => null);
            var data = netWorthController.GetNetWorth(portFolioDetails);
            ObjectResult okResult = data as ObjectResult;

            Assert.AreEqual(404, okResult.StatusCode);
            Assert.Pass();

        }

        [Test]

        public void TestForResponseWhenObjectHaveValuesinProvider()
        {
            networthProviderMock.Setup(x => x.sellAssets(_portFolioDetails)).Returns(assetSaleResponse);
            var result = netWorthController.SellAssets(_portFolioDetails);
            ObjectResult okResult = result as ObjectResult;
            Assert.AreEqual(okResult.StatusCode, 200);
        }

        [Test]
        public void TestForResponseWhenObjectDoesNotHaveValuesinProvider()
        {
            networthProviderMock.Setup(x => x.sellAssets(It.Ref<PortFolioDetails>.IsAny)).Returns(() => null);
            var result = netWorthController.SellAssets(_portFolioDetails);
            ObjectResult okResult = result as ObjectResult;
            Assert.AreEqual(okResult.StatusCode, 404);
        }


    }
}