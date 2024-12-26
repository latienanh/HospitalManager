using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManager.Abstractions.Common;
using HospitalManager.Entities;
using Volo.Abp.Identity;

namespace HospitalManager.Abstractions;

public interface IHospitalDapperRepository : IBaseDapperRepository<Hospital>
{
    Task<IEnumerable<IdentityUser>> GetUserNotInHospital(int skip, int take);
    Task<int> GetCountUserNotInHospitalAsync(int take);
}