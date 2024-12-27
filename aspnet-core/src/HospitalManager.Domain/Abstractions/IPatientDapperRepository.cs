using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManager.Abstractions.Common;
using HospitalManager.Dtos.Response;
using HospitalManager.Entities;
using Volo.Abp.Domain.Repositories;

namespace HospitalManager.Abstractions;

public interface IPatientDapperRepository : IBaseDapperRepository<Patient>
{
    Task<IEnumerable<HospitalPatientCountDto>> HospitalPatientCountReportDapper();

    Task<IEnumerable<ProvincePatientCountDto>> ProvincePatientCountReportDapper();

}