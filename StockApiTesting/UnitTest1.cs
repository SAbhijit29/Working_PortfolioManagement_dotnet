using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using StockMicroservice.Controllers;
using StockMicroservice.Models;
using StockMicroservice.Provider;
using System.Linq;
using StockMicroservice.Repository;
using Microsoft.AspNetCore.Mvc;

namespace StockApiTesting
{
    [TestFixture]
    public class Tests
    {

        List<StockDetails> stocks = new List<StockDetails>();
        private readonly StockController stockController;
        private readonly StockProvider stockProvider;

        private readonly Mock<IStockProvider> mockProvider = new Mock<IStockProvider>();
        private readonly Mock<IStockRepository> mockRepository = new Mock<IStockRepository>();

        public Tests()
        {
            stockController = new StockController(mockProvider.Object);
            stockProvider = new StockProvider(mockRepository.Object);
        }

        [SetUp]
        public void Setup()
        {
            stocks = new List<StockDetails>()
            {
                new StockDetails{ StockId = 1, StockName="Tata Power", StockValue=100},
                new StockDetails{ StockId = 2, StockName="Coal India", StockValue=200},
            };

            mockProvider.Setup(x => x.GetStockByNameProvider(It.IsAny<string>())).Returns((string s) => stocks.FirstOrDefault(x=>x.StockName.Equals(s)));
            mockRepository.Setup(x => x.GetStockByNameRepository(It.IsAny<string>())).Returns((string s) => stocks.FirstOrDefault(x => x.StockName.Equals(s)));

        }

        [Test]
        public void GetStockByNameController_PassCase()
        {
            var stock = stockController.GetStocksDetailsByName("Tata Power");
            ObjectResult result = stock as ObjectResult;
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public void GetStockByNameController_FailCase()
        {
            var stock = stockController.GetStocksDetailsByName("ABC");
            ObjectResult result = stock as ObjectResult;
            Assert.AreEqual(404, result.StatusCode);
        }

        [Test]
        public void GetStockByNameProvider_PassCase()
        {
            var stock = stockController.GetStocksDetailsByName("Coal India");
            ObjectResult result = stock as ObjectResult;
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public void GetStockByNameProvider_FailCase()
        {
            var stock = stockController.GetStocksDetailsByName("rrr");
            ObjectResult result = stock as ObjectResult;
            Assert.AreEqual(404, result.StatusCode);
        }
    }
}