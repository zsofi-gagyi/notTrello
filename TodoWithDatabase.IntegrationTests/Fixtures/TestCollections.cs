using TodoWithDatabase.IntegrationTests;
using Xunit;

namespace ferrilata_devilline.IntegrationTests.Fixtures
{
    [CollectionDefinition("BaseCollection")]
    public class Collection1 : ICollectionFixture<TestContext>
    {
    }

    [CollectionDefinition("UserCollection")] // to be used for testing user-related endpoints. Can be run in parallel with project-related endpoints. 
    public class Collection2 : ICollectionFixture<TestContext>
    {
    }

    [CollectionDefinition("ProjectCollection")] 
    public class Collection3 : ICollectionFixture<TestContext>
    {
    }
}