using DailyMutualFundNAVMicroservice.Models;
using DailyMutualFundNAVMicroservice.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyMutualFundNAVMicroservice.Provider
{
    public class MutualFundProvider : IMutualFundProvider
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(MutualFundProvider));
        private readonly IMutualFundRepository mutualFundRepository;

        public MutualFundProvider(IMutualFundRepository _mutualFundRepository)
        {
            mutualFundRepository = _mutualFundRepository;
        }

        public List<MutualFundDetails> GetAllMutual()
        {
            List<MutualFundDetails> s = new List<MutualFundDetails>();
            s = mutualFundRepository.GetAllMutual();
            return s;
        }


        public MutualFundDetails GetMutualFundByNameProvider(string mutualFundName)
        {
            MutualFundDetails mutualFundData = null;

            try
            {
                _log4net.Info("MutualFundNAVController has Called GetMutualFundByNameProvider and " + mutualFundName + " is searched in MutualFundProvider");
                mutualFundData = mutualFundRepository.GetMutualFundByNameRepository(mutualFundName);
            }
            catch (Exception e)
            {
                _log4net.Error("In MutualFundProvider encountered exception" + e.Message);
            }
            return mutualFundData;
        }
    }
}
