using HospitalManager.Samples;
using Xunit;

namespace HospitalManager.EntityFrameworkCore.Domains;

[Collection(HospitalManagerTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<HospitalManagerEntityFrameworkCoreTestModule>
{

}
