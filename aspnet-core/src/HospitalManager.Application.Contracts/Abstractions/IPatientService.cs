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

public interface IPatientService : ICrudAppService<PatientDto, int, PagedAndSortedResultRequestDto, CreateUpdatePatientDto>
{
    Task<GetPagingResponse<PatientDto>> GetPatientDapperListAsync(BaseGetPagingRequest request);
}