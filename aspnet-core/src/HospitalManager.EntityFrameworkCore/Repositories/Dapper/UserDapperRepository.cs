using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using HospitalManager.Abstractions;
using HospitalManager.Entities;
using HospitalManager.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;

namespace HospitalManager.Repositories.Dapper
{
    public class UserDapperRepository(IDbContextProvider<HospitalManagerDbContext> dbContextProvider) : BaseDapperRepository(dbContextProvider),IScopedDependency, IUserDapperRepository
    {
        public async Task<IEnumerable<string>> GetGmailByRole(string RoleName)
        {
            try
            {
                var dbConnection = await GetDbConnectionAsync();

                var query = $@"
                select Email from abpusers
                join abpuserroles
                on abpusers.Id = abpuserroles.UserId
                join abproles
                on abpuserroles.RoleId = abproles.Id
                where abproles.`Name` = @RoleName";

                var parameters = new DynamicParameters();
                parameters.Add("@RoleName", RoleName, DbType.String);
                var wards = await dbConnection.QueryAsync<string>(
                    query,
                    parameters,
                    transaction: await GetDbTransactionAsync()
                );

                return wards.ToList();
            }
            catch (Exception ex)
            {
                // Ghi lại thông tin lỗi chi tiết
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }

        public async Task<IEnumerable<IdentityUser>> GetUserInHospital(int HospitalId)
        {
            try
            {
                var dbConnection = await GetDbConnectionAsync();

                var query = $@"
                SELECT abpusers.Id,UserName,abpusers.`Name` from abpusers
                JOIN userhospitals
                on userhospitals.UserId = abpusers.Id
                where userhospitals.HospitalId = @HospitalId";

                var parameters = new DynamicParameters();
                parameters.Add("@HospitalId", HospitalId, DbType.Int32);
                var user = await dbConnection.QueryAsync<IdentityUser>(
                    query,
                    parameters,
                    transaction: await GetDbTransactionAsync()
                );

                return user.ToList();
            }
            catch (Exception ex)
            {
                // Ghi lại thông tin lỗi chi tiết
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }
    }
}
