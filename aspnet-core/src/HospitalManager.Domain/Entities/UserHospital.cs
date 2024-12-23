using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace HospitalManager.Entities
{
    public sealed class UserHospital : FullAuditedAggregateRoot<int>
    {
        public Guid UserId { get; set; }
        public int HospitalId { get; set; }
    }
}
