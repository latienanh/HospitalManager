using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManager.Entities;

namespace HospitalManager.Abstractions
{
    public interface IProvinceDapperRepository
    {
        public Task<List<Province>> GetProvinceDapperList();
    }
}
