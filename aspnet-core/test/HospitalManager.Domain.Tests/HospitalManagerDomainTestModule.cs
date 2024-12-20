using Volo.Abp.Modularity;

namespace HospitalManager;

[DependsOn(
    typeof(HospitalManagerDomainModule),
    typeof(HospitalManagerTestBaseModule)
)]
public class HospitalManagerDomainTestModule : AbpModule
{

}
