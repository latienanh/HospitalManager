using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace HospitalManager.Entities.Common;

public class BaseEntity<T> : FullAuditedAggregateRoot<int>
{
    [Required]
    [MaxLength(10)]
    public T Code { get; set; }

    [Required]
    [MaxLength(30)]
    public string Name { get; set; }
}