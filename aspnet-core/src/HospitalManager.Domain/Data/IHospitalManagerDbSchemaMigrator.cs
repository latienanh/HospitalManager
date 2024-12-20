using System.Threading.Tasks;

namespace HospitalManager.Data;

public interface IHospitalManagerDbSchemaMigrator
{
    Task MigrateAsync();
}
