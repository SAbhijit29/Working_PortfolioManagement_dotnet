using DailyMutualFundNAVMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyMutualFundNAVMicroservice.Provider
{
    public interface IMutualFundProvider
    {
        public MutualFundDetails GetMutualFundByNameProvider(string mutualFundName);
        public List<MutualFundDetails> GetAllMutual();
    }
}
