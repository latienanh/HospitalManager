﻿using HospitalManager.Abstractions;
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

namespace HospitalManager.Repositories.Dapper;

public class DistrictDapperRepository(IDbContextProvider<HospitalManagerDbContext> dbContextProvider)
    : BaseDapperRepository(dbContextProvider), IDistrictDapperRepository, IScopedDependency
{
    public async Task<List<District>> GetPagingAsync(int skip, int take, string? additionalConditions = "")
    {
        try
        {
            var dbConnection = await GetDbConnectionAsync();
            var tableName = "districts";
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

            var districts = await dbConnection.QueryAsync<District>(
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
                    FROM districts
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