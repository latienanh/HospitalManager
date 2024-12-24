using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManager.Dtos.Request.CreateUpdate.Common;

public class CreateUpdateAdministrative : BaseCreateUpdate
{

    public string AdministrativeLevel { get; set; } = string.Empty;

    public string Note { get; set; } = string.Empty;
}