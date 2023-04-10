using DailyMutualFundNAVMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyMutualFundNAVMicroservice.Repository
{
   public interface IMutualFundRepository
    {
        public MutualFundDetails GetMutualFundByNameRepository(string mutualFundName);
        public List<MutualFundDetails> GetAllMutual();
    }
}
