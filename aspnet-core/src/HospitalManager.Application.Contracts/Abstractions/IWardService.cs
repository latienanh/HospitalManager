using HospitalManager.Dtos.Request.CreateUpdate;
using HospitalManager.Dtos.Request.GetPaging;
using HospitalManager.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace HospitalManager.Abstractions;

public interface IWardService : ICrudAppService<WardDto, int, PagedAndSortedResultRequestDto, CreateUpdateWardDto>
{
    Task<GetPagingResponse<WardDto>> GetWardDapperListAsync(GetPagingWardRequest request);
}