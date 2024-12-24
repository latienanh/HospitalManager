using HospitalManager.Dtos.Request.CreateUpdate;
using HospitalManager.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HospitalManager.Dtos.Common;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace HospitalManager.Abstractions;

public interface IHospitalService : ICrudAppService<HospitalDto, int, PagedAndSortedResultRequestDto, CreateUpdateHospitalDto>
{
    Task<GetPagingResponse<HospitalDto>> GetHospitalDapperListAsync(BaseGetPagingRequest request);
}