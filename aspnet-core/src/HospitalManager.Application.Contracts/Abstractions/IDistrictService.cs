using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HospitalManager.Dtos.Request.CreateUpdate;
using HospitalManager.Dtos.Request.GetPaging;
using HospitalManager.Dtos.Response;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace HospitalManager.Abstractions;

public interface IDistrictService : ICrudAppService<DistrictDto, int, PagedAndSortedResultRequestDto, CreateUpdateDistrictDto>
{
    Task<GetPagingResponse<DistrictDto>> GetDistrictDapperListAsync(GetPagingDistrictRequest request);
}