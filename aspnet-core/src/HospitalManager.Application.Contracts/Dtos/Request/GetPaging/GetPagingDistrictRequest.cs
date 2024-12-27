using System;
using System.Collections.Generic;
using System.Text;
using HospitalManager.Dtos.Common;

namespace HospitalManager.Dtos.Request.GetPaging;

public class GetPagingDistrictRequest : BaseGetPagingRequest
{
    public int? ProvinceCode { get; set; }
}