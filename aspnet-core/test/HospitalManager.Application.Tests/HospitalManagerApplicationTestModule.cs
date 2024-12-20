using Volo.Abp.Modularity;

namespace HospitalManager;

[DependsOn(
    typeof(HospitalManagerApplicationModule),
    typeof(HospitalManagerDomainTestModule)
)]
public class HospitalManagerApplicationTestModule : AbpModule
{

}
