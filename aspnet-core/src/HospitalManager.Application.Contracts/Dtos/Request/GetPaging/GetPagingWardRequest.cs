using System;
using System.Collections.Generic;
using System.Text;
using HospitalManager.Dtos.Common;

namespace HospitalManager.Dtos.Request.GetPaging;

public class GetPagingWardRequest : BaseGetPagingRequest
{
    public int? DistrictCode { get; set; }

    public int? ProvinceCode { get; set; }
}