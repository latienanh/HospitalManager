using AutoMapper.Internal.Mappers;
using HospitalManager.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManager.Abstractions;
using HospitalManager.Dtos.Common;
using HospitalManager.Dtos.Request.CreateUpdate;
using HospitalManager.Dtos.Request.GetPaging;
using HospitalManager.Dtos.Response;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.ObjectMapping;
using OfficeOpenXml;
using Quartz.Job;
using Volo.Abp.Emailing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IO;

namespace HospitalManager.Services;

[Authorize("District_Manager")]
public class DistrictAppService(IRepository<District, int> repository,
    IDistrictDapperRepository districtDapperRepository,
    ExcelService excelService
)
    : CrudAppService<District, DistrictDto, int, PagedAndSortedResultRequestDto, CreateUpdateDistrictDto>(
        repository), IDistrictService
{
    [Authorize("District_Delete")]
    public override async Task DeleteAsync(int id)
    {
        await Repository.DeleteAsync(id);
    }
    [HttpPost]
    [Authorize("District_GetPaging")]
    public async Task<GetPagingResponse<DistrictDto>> GetDistrictDapperListAsync([FromBody] GetPagingDistrictRequest request)
    {
        string whereClause = "WHERE IsDeleted = FALSE";
        if (request.ProvinceCode!=null)
        {
            whereClause += $" AND ProvinceCode = {request.ProvinceCode}";
        }
        var districts = await districtDapperRepository.GetPagingAsync(request.Index, request.Size, whereClause);
        var totalPage = await districtDapperRepository.GetCountAsync(request.Size,whereClause);
        var mappedDistricts = ObjectMapper.Map<List<District>, List<DistrictDto>>(districts);
        var result = new GetPagingResponse<DistrictDto>
        {
            Data = mappedDistricts,
            TotalPage = totalPage
        };
        return result;
    }
    [Authorize("District_Create")]
    public override async Task<DistrictDto> CreateAsync(CreateUpdateDistrictDto input)
    {
        var checkCode = await Repository.FirstOrDefaultAsync(x => x.Code == input.Code);
        if (checkCode != null)
        {
            throw new BusinessException()
                .WithData("message", $"Mã: {input.Code} đã tồn tại trong hệ thống");
        }
        return await base.CreateAsync(input);
    }[Authorize("District_Update")]
    public override async Task<DistrictDto> UpdateAsync(int id, CreateUpdateDistrictDto input)
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
    [Authorize("District_Import")]
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
                await excelService.ImportExcelFileAsync<District>(stream, isUpdate,
                    mapRowToEntity: (worksheet, row) =>
                    {
                        return new District()
                        {
                            Code = int.Parse(worksheet.Cells[row, 1].Text),
                            Name = worksheet.Cells[row, 2].Text,
                            AdministrativeLevel = worksheet.Cells[row, 4].Text,
                            Note = "",
                            ProvinceCode = int.Parse(worksheet.Cells[row, 5].Text),
                        };
                    },
                    findExistingEntity: async code =>
                    {
                        return await repository.FirstOrDefaultAsync(x => x.Code == code);
                    },
                    insertEntity: async (IEnumerable<District> districtsAdd) =>
                    {
                        await repository.InsertManyAsync(districtsAdd);
                    },
                    updateEntity: async (IEnumerable<District> districtsUpdate) =>
                    {
                        await repository.UpdateManyAsync(districtsUpdate);
                    },
                    mapRowToEntityUpdate: (District districtEx, ExcelWorksheet worksheet, int row) =>
                    {
                        districtEx.Code = int.Parse(worksheet.Cells[row, 1].Text);
                        districtEx.Name = worksheet.Cells[row, 2].Text; ;
                        districtEx.AdministrativeLevel = worksheet.Cells[row, 4].Text;
                        districtEx.ProvinceCode = int.Parse(worksheet.Cells[row, 5].Text);
                        return districtEx;
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