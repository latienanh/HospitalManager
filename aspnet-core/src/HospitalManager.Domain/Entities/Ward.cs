using HospitalManager.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace HospitalManager.Entities;

public sealed class Ward : CommonAdministrative
{
    public string Note { get; set; } = string.Empty;

    public int DistrictCode { get; set; }

    public int ProvinceCode { get; set; }
}