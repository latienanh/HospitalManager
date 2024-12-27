using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace HospitalManager.Abstractions
{
    public interface IUserDapperRepository
    {
        Task<IEnumerable<string>> GetGmailByRole(string RoleName);
        Task<IEnumerable<IdentityUser>> GetUserInHospital(int HospitalId);
    }
}
