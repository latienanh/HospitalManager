using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManager.Abstractions;
using HospitalManager.Dtos;
using HospitalManager.Dtos.Common;
using HospitalManager.Dtos.CreateUpdate;
using HospitalManager.Dtos.Response;
using HospitalManager.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace HospitalManager.Services
{
    public class ProvinceAppService(
        IRepository<Province, int> repository,
        IProvinceDapperRepository provinceDapperRepository,
        ExcelService excelService
        )
        : CrudAppService<Province, ProvinceDto, int, PagedAndSortedResultRequestDto, CreateUpdateProvinceDto>(
            repository), IProvinceService
    {
        [HttpPost]
        public async Task<GetPagingResponse<ProvinceDto>> GetProvinceDapperListAsync([FromBody] BaseGetPagingRequest request)
        {
            var provinces = await provinceDapperRepository.GetProvinceDapperList(request.Index, request.Size, "Where isDeleted = false");
            var totalPage = await provinceDapperRepository.GetCountTask(request.Size);
            var mappedProvinces = ObjectMapper.Map<List<Province>, List<ProvinceDto>>(provinces);
            var result = new GetPagingResponse<ProvinceDto>
            {
                Data = mappedProvinces,
                TotalPage = totalPage
            };
            return result;
        }

        public override async Task DeleteAsync(int id)
        {
            await Repository.DeleteAsync(id); 
        }

        public override async Task<ProvinceDto> CreateAsync(CreateUpdateProvinceDto input)
        {
            var checkCode = await Repository.FirstOrDefaultAsync(x => x.Code == input.Code);
            if (checkCode != null)
            {
                throw new BusinessException()
                    .WithData("message", $"Mã: {input.Code} đã tồn tại trong hệ thống");  
            }

            return await base.CreateAsync(input);
        }
        public override async Task<ProvinceDto> UpdateAsync(int id, CreateUpdateProvinceDto input)
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
        public async Task<bool> ImportExcel(IFormFile file,bool isUpdate)
        {
            if (file == null || file.Length == 0)
            {
                return false;
            }

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    await excelService.ImportExcelFileAsync(stream,isUpdate);
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
}
