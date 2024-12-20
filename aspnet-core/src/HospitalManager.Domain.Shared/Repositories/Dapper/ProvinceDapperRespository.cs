using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManager.Abstractions;
using HospitalManager.Entities;

namespace HospitalManager.Repositories.Dapper
{
    public sealed class ProvinceDapperRespository : 
    {
        public Task<List<Province>> GetProvinceDapperList()
        {
            throw new NotImplementedException();
        }
    }
}
