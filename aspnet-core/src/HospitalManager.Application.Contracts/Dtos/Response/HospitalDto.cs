﻿using System;
using System.Collections.Generic;
using System.Text;
using HospitalManager.Dtos.Common;

namespace HospitalManager.Dtos.Response;

public class HospitalDto : BaseDto<int>
{
    public int ProvinceCode { get; set; }
    public int DistrictCode { get; set; }
    public int WardCode { get; set; }
    public string Address { get; set; }
}