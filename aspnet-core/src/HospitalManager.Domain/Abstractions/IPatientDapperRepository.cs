using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManager.Abstractions.Common;
using HospitalManager.Entities;

namespace HospitalManager.Abstractions;

public interface IPatientDapperRepository : IBaseDapperRepository<Patient>
{
}