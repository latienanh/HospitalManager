using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManager.Dtos.Response
{
    public class ProvincePatientCountDto
    {
        public int ProvinceCode { get; set; }
        public string Name { get; set; }
        public int CountPatient { get; set; }
    }
}
