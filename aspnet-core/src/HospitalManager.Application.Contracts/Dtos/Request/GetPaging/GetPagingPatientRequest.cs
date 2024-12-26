using HospitalManager.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManager.Dtos.Request.GetPaging
{
    public class GetPagingPatientRequest : BaseGetPagingRequest
    {
        public int? HospitalId { get; set; }
    }
}
