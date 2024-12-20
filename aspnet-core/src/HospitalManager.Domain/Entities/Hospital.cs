using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManager.Entities.Common;
using Volo.Abp.Domain.Entities.Auditing;

namespace HospitalManager.Entities
{
    public class Hospital : BaseEntity<int>
    {
        public int ProvinceCode { get; set; }
        public int DistrictCode { get; set; }
        public int WardCode{ get; set; }
        public string Address { get; set; }
    }

}
