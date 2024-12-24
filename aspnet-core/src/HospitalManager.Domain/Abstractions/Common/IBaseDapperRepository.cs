using HospitalManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManager.Abstractions.Common;

public interface IBaseDapperRepository<T>
{
    public Task<List<T>> GetPagingAsync(int skip, int take, string? additionalConditions = "");

    public Task<int> GetCountAsync(int take, string? additionalConditions = "");
}