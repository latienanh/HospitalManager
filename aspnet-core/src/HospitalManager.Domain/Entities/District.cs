using HospitalManager.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace HospitalManager.Entities
{
    public sealed class District : CommonAdministrative
    {
        public int ProvinceCode { get; set; }
        public string Note { get; set; }
    }
}
