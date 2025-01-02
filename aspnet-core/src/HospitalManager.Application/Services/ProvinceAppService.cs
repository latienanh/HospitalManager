using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManager.Abstractions;
using HospitalManager.Dtos.Common;
using HospitalManager.Dtos.Request.CreateUpdate;
using HospitalManager.Dtos.Response;
using HospitalManager.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace HospitalManager.Services;
[Authorize("Province_Manager")]
public class ProvinceAppService(
    IRepository<Province, int> repository,
    IProvinceDapperRepository provinceDapperRepository,
    ExcelService excelService,
    IObjectMapper objectMapper
)
    : CrudAppService<Province, ProvinceDto, int, PagedAndSortedResultRequestDto, CreateUpdateProvinceDto>(
        repository), IProvinceService
{
    [HttpPost]
    [Authorize("Province_GetPaging")]
    public async Task<GetPagingResponse<ProvinceDto>> GetProvinceDapperListAsync([FromBody] BaseGetPagingRequest request)
    {
        string whereClause = "WHERE IsDeleted = FALSE";
        var provinces = await provinceDapperRepository.GetPagingAsync(request.Index, request.Size, whereClause);
        var totalPage = await provinceDapperRepository.GetCountAsync(request.Size, whereClause);
        var mappedProvinces = objectMapper.Map<List<Province>, List<ProvinceDto>>(provinces);
        var result = new GetPagingResponse<ProvinceDto>
        {
            Data = mappedProvinces,
            TotalPage = totalPage
        };
        return result;
    }
    [Authorize("Province_Delete")]
    public override async Task DeleteAsync(int id)
    {
        await Repository.DeleteAsync(id);
    }
    [Authorize("Province_Create")]
    public override async Task<ProvinceDto> CreateAsync(CreateUpdateProvinceDto input)
    {
        var a = new List<Province>();
        var checkCode = await Repository.FindAsync(x => x.Code == input.Code);
        //var checkCod2e = await Repository.FirstOrDefaultAsync(x => x.Code == input.Code);
        //var checkCod3e = await Repository.GetAsync(x => x.Code == input.Code);
        if (checkCode != null)
        {
            throw new BusinessException()
                .WithData("message", $"Mã: {input.Code} đã tồn tại trong hệ thống");
        }

        return await base.CreateAsync(input);

    }
    [Authorize("Province_Update")]
    public override async Task<ProvinceDto> UpdateAsync(int id, CreateUpdateProvinceDto input)
    {
        var checkCode = await Repository.FindAsync(x => x.Code == input.Code && x.Id != id);
        if (checkCode != null)
        {
            throw new BusinessException()
                .WithData("message", $"Mã: {input.Code} đã tồn tại trong hệ thống");
        }
        return await base.UpdateAsync(id, input);
    }
    [HttpPost]
    public async Task<FileStreamResult> ExportExcel()
    {
        var fileStream = await excelService.ExportExcelFileAsync();

        // Trả về file Excel như một file đính kèm
        return new FileStreamResult(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            FileDownloadName = "Provinces.xlsx" // Đặt tên file khi tải về
        };
    }

    [HttpPost]
    [Authorize("Province_Import")]
    public async Task<bool> ImportExcel(IFormFile file, bool isUpdate)
    {
        if (file == null || file.Length == 0)
        {

            throw new BusinessException()
                .WithData("message", $"File không có gì");
        }
        var fileExtension = Path.GetExtension(file.FileName);
        if (string.IsNullOrEmpty(fileExtension) || !fileExtension.Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
        {
            throw new BusinessException()
                .WithData("message", $"File phải có định dạng xlsx");
        }

        try
        {
            await using (var stream = file.OpenReadStream())
            {
                await excelService.ImportExcelFileAsync<Province>(stream, isUpdate,
                    mapRowToEntity: (worksheet, row) =>
                    {
                        return new Province()
                        {
                            Code = int.Parse(worksheet.Cells[row, 1].Text),
                            Name = worksheet.Cells[row, 2].Text,
                            AdministrativeLevel = worksheet.Cells[row, 4].Text,
                            Note = "",
                        };
                    },
                    findExistingEntity: async code =>
                    {
                        return await repository.FindAsync(x => x.Code == code);
                    },
                    insertEntity: async (IEnumerable<Province> provincesAdd) =>
                    {
                        await repository.InsertManyAsync(provincesAdd, true);
                    },
                    updateEntity: async (IEnumerable<Province> provincesUpdate) =>
                    {
                        await repository.UpdateManyAsync(provincesUpdate, true);
                    },
                    mapRowToEntityUpdate: (Province provinceEx, ExcelWorksheet worksheet, int row) =>
                    {
                        provinceEx.Code = int.Parse(worksheet.Cells[row, 1].Text);
                        provinceEx.Name = worksheet.Cells[row, 2].Text; ;
                        provinceEx.AdministrativeLevel = worksheet.Cells[row, 4].Text;
                        return provinceEx;
                    }
                );
            }
        }
        catch (Exception ex)
        {
            // Ghi lại lỗi chi tiết vào log để dễ dàng debug
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            return false;
        }

        return true;
    }

}