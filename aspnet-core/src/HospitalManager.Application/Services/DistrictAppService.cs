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
using Volo.Abp.Domain.Repositories;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.ObjectMapping;
using OfficeOpenXml;

namespace HospitalManager.Services;

public class DistrictAppService(IRepository<District, int> repository,
    IDistrictDapperRepository districtDapperRepository,
    ExcelService excelService
)
    : CrudAppService<District, DistrictDto, int, PagedAndSortedResultRequestDto, CreateUpdateDistrictDto>(
        repository), IDistrictService
{

    public override async Task DeleteAsync(int id)
    {
        await Repository.DeleteAsync(id);
    }
    [HttpPost]
    public async Task<GetPagingResponse<DistrictDto>> GetDistrictDapperListAsync([FromBody] GetPagingDistrictRequest request)
    {
        var districts = await districtDapperRepository.GetPagingAsync(request.Index, request.Size, $"WHERE ProvinceCode = {request.ProvinceCode} and IsDeleted =FALSE");
        var totalPage = await districtDapperRepository.GetCountAsync(request.Size,$"and ProvinceCode = {request.ProvinceCode}");
        var mappedDistricts = ObjectMapper.Map<List<District>, List<DistrictDto>>(districts);
        var result = new GetPagingResponse<DistrictDto>
        {
            Data = mappedDistricts,
            TotalPage = totalPage
        };
        return result;
    }

    public override async Task<DistrictDto> CreateAsync(CreateUpdateDistrictDto input)
    {
        var checkCode = await Repository.FirstOrDefaultAsync(x => x.Code == input.Code);
        if (checkCode != null)
        {
            throw new BusinessException()
                .WithData("message", $"Mã: {input.Code} đã tồn tại trong hệ thống");
        }
        return await base.CreateAsync(input);
    }
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
    public async Task<bool> ImportExcel(IFormFile file, bool isUpdate)
    {
        if (file == null || file.Length == 0)
        {
            return false;
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