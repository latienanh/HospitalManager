﻿using System;
using System.Collections.Generic;
using System.Text;
using HospitalManager.Dtos.Common;

namespace HospitalManager.Dtos.Response;

public class WardDto : CommonAdministrativeDto
{
    public int ProvinceCode { get; set; }
    public int DistrictCode { get; set; }
}