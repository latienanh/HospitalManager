using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManager.Dtos.Common
{
    public class BaseGetPagingRequest
    {
        public int Index { get; set; }
        public int Size { get; set; }
    }
}
