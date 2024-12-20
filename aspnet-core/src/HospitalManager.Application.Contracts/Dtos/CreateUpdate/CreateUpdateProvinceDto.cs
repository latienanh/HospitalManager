using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManager.Dtos.CreateUpdate
{
    public class CreateUpdateProvinceDto
    {
        public int Code { get; set; }
        public string Name { get; set; }

        public string AdministrativeLevel { get; set; }

        public string Note { get; set; }
    }
}
