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

namespace HospitalManager.Services;

public class WardAppService(
    IRepository<Ward, int> repository,
    IWardDapperRepository WardDapperRepository,
    ExcelService excelService
)
    : CrudAppService<Ward, WardDto, int, PagedAndSortedResultRequestDto, CreateUpdateWardDto>(
        repository), IWardService
{

    public override async Task DeleteAsync(int id)
    {
        await Repository.DeleteAsync(id);
    }

    [HttpPost]
    public async Task<GetPagingResponse<WardDto>> GetWardDapperListAsync([FromBody] GetPagingWardRequest request)
    {
        var Wards = await WardDapperRepository.GetPagingAsync(request.Index, request.Size,
            $"WHERE ProvinceCode ={request.ProvinceCode} and DistrictCode = {request.DistrictCode} and IsDeleted =FALSE");
        var totalPage = await WardDapperRepository.GetCountAsync(request.Size, $"and ProvinceCode = {request.ProvinceCode} and DistrictCode = {request.DistrictCode}");
        var mappedWards = ObjectMapper.Map<List<Ward>, List<WardDto>>(Wards);
        var result = new GetPagingResponse<WardDto>
        {
            Data = mappedWards,
            TotalPage = totalPage
        };
        return result;
    }

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

    public override async Task<WardDto> UpdateAsync(int id, CreateUpdateWardDto input)
    {
        var checkCode = await Repository.FirstOrDefaultAsync(x => x.Code == input.Code);
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
                await excelService.ImportExcelFileAsync<Ward>(stream, isUpdate,
                    mapRowToEntity: (worksheet, row) =>
                    {
                        return new Ward()
                        {
                            Code = int.Parse(worksheet.Cells[row, 1].Text),
                            Name = worksheet.Cells[row, 2].Text,
                            AdministrativeLevel = worksheet.Cells[row, 4].Text,
                            Note = "",
                            DistrictCode = int.Parse(worksheet.Cells[2, 5].Text),
                            ProvinceCode = int.Parse(worksheet.Cells[row, 7].Text),
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
                        wardEx.Code = int.Parse(worksheet.Cells[row, 1].Text);
                        wardEx.Name = worksheet.Cells[row, 2].Text; ;
                        wardEx.AdministrativeLevel = worksheet.Cells[row, 4].Text;
                        wardEx.DistrictCode = int.Parse(worksheet.Cells[2, 5].Text);
                        wardEx.ProvinceCode = int.Parse(worksheet.Cells[row, 7].Text);
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