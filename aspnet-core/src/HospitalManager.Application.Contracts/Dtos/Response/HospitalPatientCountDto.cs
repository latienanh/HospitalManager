using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManager.Dtos.Response
{
    public class HospitalPatientCountDto
    {
        public int HospitalId { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public int CountPatient { get; set; }
    }
}
