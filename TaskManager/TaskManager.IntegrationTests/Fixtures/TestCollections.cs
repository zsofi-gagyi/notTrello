using TodoWithDatabase.IntegrationTests;
using Xunit;

namespace ferrilata_devilline.IntegrationTests.Fixtures
{
    [CollectionDefinition("BaseCollection")]
    public class Collection1 : ICollectionFixture<TestContext>
    {
    }

    [CollectionDefinition("UserCollection")] 
    public class Collection2 : ICollectionFixture<TestContext>
    {
    }

    [CollectionDefinition("DeleteUserCollection")]
    public class Collection3 : ICollectionFixture<TestContext>
    {
    }

    [CollectionDefinition("ProjectsCollection")] 
    public class Collection4 : ICollectionFixture<TestContext>
    {
    }

    [CollectionDefinition("UnauthorizedCollection")]
    public class Collection5 : ICollectionFixture<TestContext>
    {
    }
}