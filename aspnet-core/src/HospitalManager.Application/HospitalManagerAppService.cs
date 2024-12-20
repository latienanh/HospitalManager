using System;
using System.Collections.Generic;
using System.Text;
using HospitalManager.Localization;
using Volo.Abp.Application.Services;

namespace HospitalManager;

/* Inherit your application services from this class.
 */
public abstract class HospitalManagerAppService : ApplicationService
{
    protected HospitalManagerAppService()
    {
        LocalizationResource = typeof(HospitalManagerResource);
    }
}
