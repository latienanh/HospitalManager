using Volo.Abp.Modularity;

namespace HospitalManager;

/* Inherit from this class for your domain layer tests. */
public abstract class HospitalManagerDomainTestBase<TStartupModule> : HospitalManagerTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
