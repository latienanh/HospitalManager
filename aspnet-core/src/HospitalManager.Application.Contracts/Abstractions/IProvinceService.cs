using System;
using System.Collections.Generic;
using System.Text;
using HospitalManager.Dtos;
using HospitalManager.Dtos.CreateUpdate;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace HospitalManager.Abstractions
{
    public interface IProvinceService : ICrudAppService<ProvinceDto, int, PagedAndSortedResultRequestDto, CreateUpdateProvinceDto>
    {
    }
}
