using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace HospitalManager.Dtos.Common;

public class BaseDto<T> : AuditedEntityDto<int>
{
    public T Code { get; set; }

    public string Name { get; set; }
}