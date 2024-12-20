using Xunit;

namespace HospitalManager.EntityFrameworkCore;

[CollectionDefinition(HospitalManagerTestConsts.CollectionDefinitionName)]
public class HospitalManagerEntityFrameworkCoreCollection : ICollectionFixture<HospitalManagerEntityFrameworkCoreFixture>
{

}
