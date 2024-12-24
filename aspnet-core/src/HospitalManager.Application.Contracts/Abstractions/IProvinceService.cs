using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HospitalManager.Dtos.Common;
using HospitalManager.Dtos.Request.CreateUpdate;
using HospitalManager.Dtos.Response;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace HospitalManager.Abstractions;

public interface IProvinceService : ICrudAppService<ProvinceDto, int, PagedAndSortedResultRequestDto, CreateUpdateProvinceDto>
{
    Task<GetPagingResponse<ProvinceDto>> GetProvinceDapperListAsync( BaseGetPagingRequest request);
}