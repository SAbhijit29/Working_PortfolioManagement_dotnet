using NUnit.Framework;
using Moq;
using DailyMutualFundNAVMicroservice.Models;
using DailyMutualFundNAVMicroservice.Controllers;
using DailyMutualFundNAVMicroservice.Repository;
using DailyMutualFundNAVMicroservice.Provider;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace MutualFundNAVTest
{
    [TestFixture]
    public class Tests
    {
        List<MutualFundDetails> mutualFunds = new List<MutualFundDetails>();
        private readonly MutualFundNAVController mutualFundNAVController;
        private readonly MutualFundProvider mutualFundProvider;

        private readonly Mock<IMutualFundRepository> mockRepository = new Mock<IMutualFundRepository>();
        private readonly Mock<IMutualFundProvider> mockProvider = new Mock<IMutualFundProvider>();

        public Tests()
        {
            mutualFundNAVController = new MutualFundNAVController(mockProvider.Object);
            mutualFundProvider = new MutualFundProvider(mockRepository.Object);
        }
        [SetUp]
        public void Setup()
        {
            mutualFunds = new List<MutualFundDetails>()
            {
                new MutualFundDetails{MutualFundId=11, MutualFundName="XYZ", MutualFundValue=50},
                new MutualFundDetails{MutualFundId=22, MutualFundName="ABC", MutualFundValue=100}
            };

            mockProvider.Setup(a => a.GetMutualFundByNameProvider(It.IsAny<string>()))
                .Returns((string s) => mutualFunds.FirstOrDefault(a => a.MutualFundName.Equals(s)));

            mockRepository.Setup(a => a.GetMutualFundByNameRepository(It.IsAny<string>()))
                .Returns((string s) => mutualFunds.FirstOrDefault(a => a.MutualFundName.Equals(s)));
        }

        [TestCase("XYZ")]
        public void GetMutualFundDetailsByNameController_Pass_Result (string s)
        {
            var mf = mutualFundNAVController.GetMutualFundDetailsByName(s);
            ObjectResult result = mf as ObjectResult;
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestCase("Fail")]
        public void GetMutualFundDetailsByNameController_Fail_Result(string s)
        {
            var mf = mutualFundNAVController.GetMutualFundDetailsByName(s);
            ObjectResult result = mf as ObjectResult;
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestCase("XYZ")]
        public void GetMutualFundDetailsByNameProvider_Pass_Result(string s)
        {
            var mf = mutualFundProvider.GetMutualFundByNameProvider(s);
            Assert.IsNotNull(mf);
        }
        [TestCase("BYEBYE")]
        public void GetMutualFundDetailsByNameProvider_Fail_Result(string s)
        {
            var mf = mutualFundProvider.GetMutualFundByNameProvider(s);
            Assert.IsNull(mf);
        }
    }
}