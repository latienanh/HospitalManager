using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace HospitalManager;

[Dependency(ReplaceServices = true)]
public class HospitalManagerBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "HospitalManager";
}
