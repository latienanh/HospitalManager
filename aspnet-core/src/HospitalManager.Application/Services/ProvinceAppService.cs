using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManager.Abstractions;
using HospitalManager.Dtos;
using HospitalManager.Dtos.CreateUpdate;
using HospitalManager.Entities;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace HospitalManager.Services
{
    public class ProvinceAppService : CrudAppService<Province,ProvinceDto,int, PagedAndSortedResultRequestDto,CreateUpdateProvinceDto>,IProvinceService
    {
        public ProvinceAppService(IRepository<Province, int> repository) : base(repository)
        {
        }
    }
}
