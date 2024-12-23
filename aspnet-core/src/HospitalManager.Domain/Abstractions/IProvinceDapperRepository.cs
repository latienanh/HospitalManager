using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManager.Entities;
using Volo.Abp.Domain.Repositories;

namespace HospitalManager.Abstractions
{
    public interface IProvinceDapperRepository
    {
        public Task<List<Province>> GetProvinceDapperList(int skip, int take, string? additionalConditions = "");

        public Task<int> GetCountTask(int take, string? additionalConditions = "");
    }
}
