using DailyMutualFundNAVMicroservice.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyMutualFundNAVMicroservice.Repository
{
    public class MutualFundRepository : IMutualFundRepository
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(MutualFundRepository));
        private static List<MutualFundDetails> listOfFunds = new()
        {
            new MutualFundDetails { MutualFundId = 1, MutualFundName = "SBI", MutualFundValue = 150},
            new MutualFundDetails { MutualFundId = 2, MutualFundName = "KOTAK", MutualFundValue = 110},
            new MutualFundDetails { MutualFundId = 2, MutualFundName = "RELIANCE", MutualFundValue = 88}
        };
        public List<MutualFundDetails> GetAllMutual()
        {
            List<MutualFundDetails> s = new List<MutualFundDetails>();
            try
            {
                foreach (MutualFundDetails item in listOfFunds)
                {
                    s.Add(item);
                }
            }
            catch (Exception e)
            {

                _log4net.Error("Exception encountered while fetching all MutualFund details " + e.Message);
                return null;
            }
            return s;
        }


        public MutualFundDetails GetMutualFundByNameRepository(string mutualFundName)
        {
            MutualFundDetails mutualFundData = null;
            try
            {
                string mutualfundName = mutualFundName.ToUpper();
                _log4net.Info("In MutualFundRepository, MutualFundProvider has Called GetMutualFundByNameRepository and " + mutualFundName + " is searched");
                mutualFundData = listOfFunds.Where(e => e.MutualFundName == mutualfundName).FirstOrDefault();
                if (mutualFundData != null)
                {
                    var jsonFund = JsonConvert.SerializeObject(mutualFundData);
                    _log4net.Info("Mutual Fund Found " + jsonFund);
                }
                else
                {
                    _log4net.Info("Mutual fund " + mutualfundName + " not found in MutualFundRepository");
                }
            }
            catch (Exception e)
            {
                _log4net.Error("Exception encountered while finding mutualFund by name " + e.Message);
            }
            return mutualFundData;
        }
    }
}
