using System;
using System.Collections.Generic;
using System.Text;
using HospitalManager.Dtos.Request.CreateUpdate.Common;

namespace HospitalManager.Dtos.Request.CreateUpdate;

public class CreateUpdateDistrictDto : CreateUpdateAdministrative
{
    public int ProvinceCode { get; set; }
}