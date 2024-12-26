using HospitalManager.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManager.Entities;
using HospitalManager.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Dapper;
using Volo.Abp.Identity;

namespace HospitalManager.Repositories.Dapper;

public class HospitalDapperRepository(IDbContextProvider<HospitalManagerDbContext> dbContextProvider) : BaseDapperRepository(dbContextProvider), IHospitalDapperRepository, IScopedDependency
{
    public async Task<List<Hospital>> GetPagingAsync(int skip, int take, string? additionalConditions = "")
    {
        try
        {
            var dbConnection = await GetDbConnectionAsync();
            var tableName = "hospitals";
            // Bước 1: Tạo câu truy vấn cơ bản lấy danh sách các cột
            var baseQuery = BuildBaseSelectQuery(tableName);

            // Bước 2: Xây dựng phần SELECT thực tế với điều kiện phân trang
            var paginationQuery = BuildPaginationQuery(skip, take);

            // Bước 3: Cộng thêm các điều kiện vào câu truy vấn (nếu có)
            var finalQuery = $@"
                {baseQuery}
                SET @sql = CONCAT('SELECT ', @sql, ' FROM {tableName} ', '{additionalConditions} ', '{paginationQuery}');
                PREPARE stmt FROM @sql;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;";

            var districts = await dbConnection.QueryAsync<Hospital>(
                finalQuery,
                transaction: await GetDbTransactionAsync()
            );

            return districts.ToList();
        }
        catch (Exception ex)
        {
            // Ghi lại thông tin lỗi chi tiết
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            throw;
        }
    }

    public async Task<int> GetCountAsync(int take, string? additionalConditions = "")
    {
        try
        {
            var dbConnection = await GetDbConnectionAsync();


            additionalConditions = additionalConditions?.Replace("'", "''");


            var query = $@"
                    SELECT COUNT(*)
                    FROM hospitals
                    WHERE IsDeleted = FALSE
                    {additionalConditions}";
            var count = await dbConnection.ExecuteScalarAsync<int>(
                query,
                transaction: await GetDbTransactionAsync()
            );
            var result = (int)Math.Ceiling((decimal)count / take);
            return result;
        }
        catch (Exception ex)
        {
            // Ghi lại thông tin lỗi chi tiết
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            throw;
        }
    }

    public async Task<IEnumerable<IdentityUser>> GetUserNotInHospital(int skip, int take)
    {
        try
        {
            var dbConnection = await GetDbConnectionAsync();


            var query = $@"
                   select Id,UserName,Name FROM abpusers
                    join abpuserroles
                    on abpusers.Id = abpuserroles.UserId
                    WHERE 
                    abpuserroles.RoleId = '3a171567-67c2-1579-f1eb-9f96caab15af' and abpusers.Id NOT IN (
	                    SELECT userhospitals.UserId FROM userhospitals
                    )
                    LIMIT {take} OFFSET {skip * take}    
                    ";
            //'3a171567-67c2-1579-f1eb-9f96caab15af'
            //'3a17144e-7dd6-a0b2-45a7-45c1bddbf497'

            var user = await dbConnection.QueryAsync<IdentityUser>(
                query,
                transaction: await GetDbTransactionAsync()
            );
            return user;
        }
        catch (Exception ex)
        {
            // Ghi lại thông tin lỗi chi tiết
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            throw;
        }
    }
    public async  Task<int> GetCountUserNotInHospitalAsync(int take)
    {
        try
        {
            var dbConnection = await GetDbConnectionAsync();


            var query = $@"
                   select COUNT(*) FROM abpusers
                    join abpuserroles
                    on abpusers.Id = abpuserroles.UserId
                    WHERE 
                    abpuserroles.RoleId = '3a171567-67c2-1579-f1eb-9f96caab15af' and abpusers.Id NOT IN (
	                    SELECT userhospitals.UserId FROM userhospitals
                    )
                    ";

            var count = await dbConnection.ExecuteScalarAsync<int>(
                query,
                transaction: await GetDbTransactionAsync()
            );
            var result = (int)Math.Ceiling((decimal)count / take);
            return result;
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