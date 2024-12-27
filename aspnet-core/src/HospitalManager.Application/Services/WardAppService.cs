using AutoMapper.Internal.Mappers;
using HospitalManager.Abstractions;
using HospitalManager.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManager.Dtos.Request.CreateUpdate;
using HospitalManager.Dtos.Request.GetPaging;
using HospitalManager.Dtos.Response;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp;
using OfficeOpenXml;
using Microsoft.AspNetCore.Authorization;
using System.IO;

namespace HospitalManager.Services;
[Authorize("Ward_Manager")]
public class WardAppService(
    IRepository<Ward, int> repository,
    IWardDapperRepository WardDapperRepository,
    ExcelService excelService
)

    : CrudAppService<Ward, WardDto, int, PagedAndSortedResultRequestDto, CreateUpdateWardDto>(
        repository), IWardService
{
    [Authorize("Ward_Delete")]
    public override async Task DeleteAsync(int id)
    {
        await Repository.DeleteAsync(id);
    }

    [HttpPost]
    [Authorize("Ward_GetPaging")]
    public async Task<GetPagingResponse<WardDto>> GetWardDapperListAsync([FromBody] GetPagingWardRequest request)
    {
        string whereClause = "WHERE IsDeleted = FALSE";
        if (request.ProvinceCode != null)
        {
            whereClause += $" AND ProvinceCode = {request.ProvinceCode}";
        }
        if (request.DistrictCode != null)
        {
            whereClause += $" AND DistrictCode = {request.DistrictCode}";
        }
        var Wards = await WardDapperRepository.GetPagingAsync(request.Index, request.Size, whereClause);
        var totalPage = await WardDapperRepository.GetCountAsync(request.Size, whereClause);
        var mappedWards = ObjectMapper.Map<List<Ward>, List<WardDto>>(Wards);
        var result = new GetPagingResponse<WardDto>
        {
            Data = mappedWards,
            TotalPage = totalPage
        };
        return result;
    }
    [Authorize("Ward_Create")]
    public override async Task<WardDto> CreateAsync(CreateUpdateWardDto input)
    {
        var checkCode = await Repository.FirstOrDefaultAsync(x => x.Code == input.Code);
        if (checkCode != null)
        {
            throw new BusinessException()
                .WithData("message", $"Mã: {input.Code} đã tồn tại trong hệ thống");
        }

        return await base.CreateAsync(input);
    }
    [Authorize("Ward_Update")]
    public override async Task<WardDto> UpdateAsync(int id, CreateUpdateWardDto input)
    {
        var checkCode = await Repository.FirstOrDefaultAsync(x => x.Code == input.Code && x.Id != id);
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
    [Authorize("Ward_Import")]
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
                await excelService.ImportExcelFileAsync<Ward>(stream, isUpdate,
                    mapRowToEntity: (worksheet, row) =>
                    {
                        return new Ward()
                        {
                            Code = string.IsNullOrEmpty(worksheet.Cells[row, 1].Text) ? 0 : int.TryParse(worksheet.Cells[row, 1].Text, out var code) ? code : 0,
                            Name = string.IsNullOrEmpty(worksheet.Cells[row, 2].Text) ? string.Empty : worksheet.Cells[row, 2].Text,
                            AdministrativeLevel = string.IsNullOrEmpty(worksheet.Cells[row, 4].Text) ? string.Empty : worksheet.Cells[row, 4].Text,
                            Note = string.Empty, // Always an empty string
                            DistrictCode = string.IsNullOrEmpty(worksheet.Cells[row, 5].Text) ? 0 : int.TryParse(worksheet.Cells[row, 5].Text, out var districtCode) ? districtCode : 0,
                            ProvinceCode = string.IsNullOrEmpty(worksheet.Cells[row, 7].Text) ? 0 : int.TryParse(worksheet.Cells[row, 7].Text, out var provinceCode) ? provinceCode : 0,


                        };
                    },
                    findExistingEntity: async code =>
                    {
                        return await repository.FirstOrDefaultAsync(x => x.Code == code);
                    },
                    insertEntity: async (IEnumerable<Ward> WardsAdd) =>
                    {
                        await repository.InsertManyAsync(WardsAdd);
                    },
                    updateEntity: async (IEnumerable<Ward> WardsUpdate) =>
                    {
                        await repository.UpdateManyAsync(WardsUpdate);
                    },
                    mapRowToEntityUpdate: (Ward wardEx, ExcelWorksheet worksheet, int row) =>
                    {
                        wardEx.Code = string.IsNullOrEmpty(worksheet.Cells[row, 1].Text) ? 0 : int.TryParse(worksheet.Cells[row, 1].Text, out var code) ? code : 0;
                        wardEx.Name = string.IsNullOrEmpty(worksheet.Cells[row, 2].Text) ? string.Empty : worksheet.Cells[row, 2].Text;
                        wardEx.AdministrativeLevel = string.IsNullOrEmpty(worksheet.Cells[row, 4].Text) ? string.Empty : worksheet.Cells[row, 4].Text;
                        wardEx.Note = string.Empty;
                        wardEx.DistrictCode = string.IsNullOrEmpty(worksheet.Cells[row, 5].Text) ? 0 : int.TryParse(worksheet.Cells[row, 5].Text, out var districtCode) ? districtCode : 0;
                        wardEx.ProvinceCode = string.IsNullOrEmpty(worksheet.Cells[row, 7].Text) ? 0 : int.TryParse(worksheet.Cells[row, 7].Text, out var provinceCode) ? provinceCode : 0;


                        return wardEx;
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