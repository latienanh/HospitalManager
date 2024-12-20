using Volo.Abp.Modularity;

namespace HospitalManager;

public abstract class HospitalManagerApplicationTestBase<TStartupModule> : HospitalManagerTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
