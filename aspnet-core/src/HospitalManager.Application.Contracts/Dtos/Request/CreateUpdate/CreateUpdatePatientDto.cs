using System;
using System.Collections.Generic;
using System.Text;
using HospitalManager.Dtos.Request.CreateUpdate.Common;

namespace HospitalManager.Dtos.Request.CreateUpdate;

public class CreateUpdatePatientDto : BaseCreateUpdate
{
    public int ProvinceCode { get; set; }
    public int DistrictCode { get; set; }
    public int WardCode { get; set; }
    public string Address { get; set; } = string.Empty;

    public int? HospitalId { get; set; }
}