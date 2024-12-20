using HospitalManager.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace HospitalManager.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class HospitalManagerController : AbpControllerBase
{
    protected HospitalManagerController()
    {
        LocalizationResource = typeof(HospitalManagerResource);
    }
}
