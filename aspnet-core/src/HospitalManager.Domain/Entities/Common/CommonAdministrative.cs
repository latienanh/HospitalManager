using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace HospitalManager.Entities.Common
{
    public class CommonAdministrative : BaseEntity<int>
    {
        [Required]
        [MaxLength(30)]
        public string AdministrativeLevel { get; set; }
    }
}
