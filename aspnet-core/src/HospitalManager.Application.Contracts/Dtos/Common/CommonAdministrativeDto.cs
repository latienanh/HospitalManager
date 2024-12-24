using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManager.Dtos.Common;

public class CommonAdministrativeDto : BaseDto<int>
{
    public string AdministrativeLevel { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
}