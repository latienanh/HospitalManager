using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace HospitalManager.Dtos.Response;

public class UserHospitalDto : AuditedEntityDto<int>
{
    public Guid UserId { get; set; }
    public int HospitalId { get; set; }
}