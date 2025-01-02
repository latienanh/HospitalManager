using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManager.Abstractions;
using HospitalManager.Dtos.Request.CreateUpdate;
using HospitalManager.Dtos.Response;
using HospitalManager.Entities;
using HospitalManager.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace HospitalManager.Service.Test
{
    public class ProvinceServiceCustom(IRepository<Province, int> repository, IProvinceDapperRepository provinceDapperRepository, ExcelService excelService, IObjectMapper objectMapper) : ProvinceAppService(repository, provinceDapperRepository, excelService, objectMapper)
    {

        protected override Province MapToEntity(CreateUpdateProvinceDto createInput)
        {
            return objectMapper.Map<CreateUpdateProvinceDto, Province>(createInput);
        }

        protected override Task<ProvinceDto> MapToGetOutputDtoAsync(Province entity)
        {
            return Task.FromResult(objectMapper.Map<Province,ProvinceDto>(entity));
        }

        protected override ProvinceDto MapToGetOutputDto(Province entity)
        {
            return objectMapper.Map<Province, ProvinceDto>(entity);
        }

        protected override void MapToEntity(CreateUpdateProvinceDto updateInput, Province entity)
        {
             objectMapper.Map(updateInput,entity);
        }

        protected override Task MapToEntityAsync(CreateUpdateProvinceDto updateInput, Province entity)
        {
            objectMapper.Map(updateInput, entity);
            return Task.CompletedTask;
        }
    }
}
