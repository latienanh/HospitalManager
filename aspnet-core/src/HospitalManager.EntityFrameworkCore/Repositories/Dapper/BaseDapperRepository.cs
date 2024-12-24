using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using HospitalManager.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.Dapper;
using Volo.Abp.EntityFrameworkCore;

namespace HospitalManager.Repositories.Dapper;

public class BaseDapperRepository(IDbContextProvider<HospitalManagerDbContext> dbContextProvider)
    : DapperRepository<HospitalManagerDbContext>(dbContextProvider)
{
    // Phương thức này chỉ xây dựng phần cột cần lấy và bảng
    public string BuildBaseSelectQuery(string tableName)
    {
        // Kiểm tra và làm sạch tên bảng để tránh SQL injection
        var safeTableName = SanitizeTableName(tableName);

        return $@"
            SET @sql = NULL;
            SELECT GROUP_CONCAT(DISTINCT
                CONCAT('`', COLUMN_NAME, '`')
                ORDER BY ORDINAL_POSITION
            ) INTO @sql
            FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_SCHEMA = DATABASE()
              AND TABLE_NAME = '{safeTableName}'
              AND COLUMN_NAME != 'ExtraProperties';";
    }

    // Phương thức này chỉ xây dựng phần `LIMIT` và `OFFSET` cho câu truy vấn động
    public string BuildPaginationQuery(int skip, int take)
    {
        return $" LIMIT {take} OFFSET {skip*take}";
    }

    private string SanitizeTableName(string tableName)
    {
        // Thực hiện các biện pháp an toàn để tránh SQL injection
        return tableName.Replace("'", "").Replace(";", "");
    }

}