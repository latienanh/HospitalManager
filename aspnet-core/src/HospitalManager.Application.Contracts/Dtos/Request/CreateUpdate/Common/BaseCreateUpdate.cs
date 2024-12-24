using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManager.Dtos.Request.CreateUpdate.Common;

public class BaseCreateUpdate
{
    public int Code { get; set; }
    public string Name { get; set; } = string.Empty;
}