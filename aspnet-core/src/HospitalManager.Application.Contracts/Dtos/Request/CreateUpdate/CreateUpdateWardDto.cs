using System;
using System.Collections.Generic;
using System.Text;
using HospitalManager.Dtos.Request.CreateUpdate.Common;

namespace HospitalManager.Dtos.Request.CreateUpdate;

public class CreateUpdateWardDto : CreateUpdateAdministrative
{
    public int DistrictCode { get; set; }

    public int ProvinceCode { get; set; }
}