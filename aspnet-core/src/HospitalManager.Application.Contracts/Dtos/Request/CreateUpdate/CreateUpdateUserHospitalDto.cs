using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManager.Dtos.Request.CreateUpdate;

public class CreateUpdateUserHospitalDto
{
    public Guid UserId { get; set; }
    public int HospitalId { get; set; }
}