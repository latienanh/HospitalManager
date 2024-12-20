using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace HospitalManager.Data;

/* This is used if database provider does't define
 * IHospitalManagerDbSchemaMigrator implementation.
 */
public class NullHospitalManagerDbSchemaMigrator : IHospitalManagerDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
