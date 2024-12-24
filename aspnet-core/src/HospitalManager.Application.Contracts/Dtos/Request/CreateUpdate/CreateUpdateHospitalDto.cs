using HospitalManager.Dtos.Request.CreateUpdate.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManager.Dtos.Request.CreateUpdate;

public class CreateUpdateHospitalDto : BaseCreateUpdate
{
    public int ProvinceCode { get; set; }
    public int DistrictCode { get; set; }
    public int WardCode { get; set; }
    public string Address { get; set; } =string.Empty;
}