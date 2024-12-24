using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using HospitalManager.Abstractions;
using HospitalManager.Entities;
using HospitalManager.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace HospitalManager.Repositories.Dapper;

public class WardDapperRepository(IDbContextProvider<HospitalManagerDbContext> dbContextProvider)
    : BaseDapperRepository(dbContextProvider), IWardDapperRepository, IScopedDependency
{
    public async Task<List<Ward>> GetPagingAsync(int skip, int take, string? additionalConditions = "")
    {
        try
        {
            var dbConnection = await GetDbConnectionAsync();
            var tableName = "wards";
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

            var wards = await dbConnection.QueryAsync<Ward>(
                finalQuery,
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

    public async Task<int> GetCountAsync(int take, string? additionalConditions = "")
    {
        try
        {
            var dbConnection = await GetDbConnectionAsync();


            additionalConditions = additionalConditions?.Replace("'", "''");


            var query = $@"
                    SELECT COUNT(*)
                    FROM wards
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
}