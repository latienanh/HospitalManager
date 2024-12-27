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
using HospitalManager.Dtos.Response;

namespace HospitalManager.Repositories.Dapper;

public class PatientDapperRepository(IDbContextProvider<HospitalManagerDbContext> dbContextProvider) : BaseDapperRepository(dbContextProvider), IPatientDapperRepository, IScopedDependency
{
    public async Task<List<Patient>> GetPagingAsync(int skip, int take, string? additionalConditions = "")
    {
        try
        {
            var dbConnection = await GetDbConnectionAsync();
            var tableName = "patients";
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

            var districts = await dbConnection.QueryAsync<Patient>(
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
                     FROM patients
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

    public async Task<IEnumerable<HospitalPatientCountDto>> HospitalPatientCountReportDapper()
    {
        try
        {
            var dbConnection = await GetDbConnectionAsync();


            var query = $@"
                     SELECT patients.HospitalId,hospitals.`Code`,hospitals.`Name` ,COUNT(*) as CountPatient
                        FROM patients
                        JOIN hospitals 
                        ON patients.HospitalId = hospitals.Id
                        WHERE DATE(patients.CreationTime) = CURDATE() and patients.IsDeleted = FALSE

                        GROUP BY patients.HospitalId,hospitals.`Code`,hospitals.`Name`";

            var data = await dbConnection.QueryAsync<HospitalPatientCountDto>(
                query,
                transaction: await GetDbTransactionAsync()
            );
            
            return data;
        }
        catch (Exception ex)
        {
            // Ghi lại thông tin lỗi chi tiết
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            throw;
        }
    }

    public async Task<IEnumerable<ProvincePatientCountDto>> ProvincePatientCountReportDapper()
    {
        try
        {
            var dbConnection = await GetDbConnectionAsync();

            var query = $@"
                    SELECT patients.ProvinceCode,provinces.`Name` ,COUNT(*) as CountPatient
                    FROM patients
                    JOIN provinces 
                    ON patients.ProvinceCode = provinces.`Code`
                    WHERE DATE(patients.CreationTime) = CURDATE() and patients.IsDeleted = FALSE

                    GROUP BY patients.ProvinceCode,provinces.`Name`";

            var data = await dbConnection.QueryAsync<ProvincePatientCountDto>(
                query,
                transaction: await GetDbTransactionAsync()
            );
            return data;
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