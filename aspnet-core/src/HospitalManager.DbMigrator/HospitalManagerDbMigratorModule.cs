using HospitalManager.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace HospitalManager.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(HospitalManagerEntityFrameworkCoreModule),
    typeof(HospitalManagerApplicationContractsModule)
    )]
public class HospitalManagerDbMigratorModule : AbpModule
{
}
