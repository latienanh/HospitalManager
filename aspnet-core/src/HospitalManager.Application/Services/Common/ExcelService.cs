using HospitalManager.Entities;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Domain.Repositories;

public class ExcelService : IScopedDependency 
{
    public List<Province> GetSampleData()
    {
        try
        {
            var provinces = new List<Province>
            {
                new Province { Name = "Hà Nội", Code = 1, IsDeleted = false },
                new Province { Name = "Hồ Chí Minh", Code = 2, IsDeleted = false },
                new Province { Name = "Đà Nẵng", Code = 3, IsDeleted = false },
                new Province { Name = "Hải Phòng", Code = 4, IsDeleted = true }
            };
            return provinces;
        }
        catch (Exception ex)
        {
            // Log the exception (you can replace this with a proper logging mechanism)
            Console.WriteLine($"An error occurred while retrieving sample data: {ex.Message}");
            throw new Exception("An error occurred while retrieving sample data.", ex);
        }
    }

    public async Task<MemoryStream> ExportExcelFileAsync()
    {
        var memoryStream = new MemoryStream();
        try
        {
            var provinces = GetSampleData(); // Lấy dữ liệu mẫu

            // Tạo file Excel
            using (var package = new ExcelPackage(memoryStream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Provinces");
                worksheet.Cells[1, 1].Value = "Name";
                worksheet.Cells[1, 2].Value = "Code";
                worksheet.Cells[1, 3].Value = "IsDeleted";

                for (int i = 0; i < provinces.Count; i++)
                {
                    var province = provinces[i];
                    worksheet.Cells[i + 2, 1].Value = province.Name;
                    worksheet.Cells[i + 2, 2].Value = province.Code;
                    worksheet.Cells[i + 2, 3].Value = province.IsDeleted;
                }

                await package.SaveAsync();
            }

            memoryStream.Position = 0;
            return memoryStream;
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"An error occurred while exporting the Excel file: {ex.Message}");
            throw new Exception("An error occurred while generating the Excel file.", ex);
        }
    }

    public async Task ImportExcelFileAsync<T>(
        Stream fileStream,
        bool isUpdate,
        Func<ExcelWorksheet,int, T> mapRowToEntity,
        Func<int, Task<T>> findExistingEntity,  
        Func<IEnumerable<T>, Task> insertEntity,
        Func<IEnumerable<T>, Task> updateEntity ,
        Func<T,ExcelWorksheet, int, T> mapRowToEntityUpdate) where T : FullAuditedAggregateRoot<int>

    {
        var entitiesToAdd = new List<T>();
        var entitiesToUpdate = new List<T>();

        try
        {
            using (var package = new ExcelPackage(fileStream))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount - 1; row++)
                {
                    var code = int.Parse(worksheet.Cells[row, 1].Text);

                    // Check if the province code already exists in the database
                    var existinEntity = await findExistingEntity(code);
                    if (existinEntity != null)
                    {
                        if (isUpdate)
                        {
                            var entityUpdate = mapRowToEntityUpdate(existinEntity,worksheet,row);
                            entitiesToUpdate.Add(entityUpdate);
                        }
                        
                        continue;
                    }

                    var entity = mapRowToEntity(worksheet, row);


                    entitiesToAdd.Add(entity);
                }
            }

            if (entitiesToAdd.Any())
            {
                await insertEntity(entitiesToAdd);

            }

            if (isUpdate&&entitiesToUpdate.Any())
            {
                await updateEntity(entitiesToUpdate);
            }
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"An error occurred while importing the Excel file: {ex.Message}");
            throw new Exception("An error occurred while importing the Excel file.", ex);
        }
    }

   

}
