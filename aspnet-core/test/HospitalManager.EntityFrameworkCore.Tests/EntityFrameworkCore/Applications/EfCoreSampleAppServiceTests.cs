using HospitalManager.Samples;
using Xunit;

namespace HospitalManager.EntityFrameworkCore.Applications;

[Collection(HospitalManagerTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<HospitalManagerEntityFrameworkCoreTestModule>
{

}
