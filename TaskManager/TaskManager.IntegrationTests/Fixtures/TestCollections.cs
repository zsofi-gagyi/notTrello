using TaskManager.IntegrationTests.Fixtures.Context;
using Xunit;

namespace TaskManager.IntegrationTests.Fixtures
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

    [CollectionDefinition("AlwaysAuthenticatedCollection")]
    public class Collection6 : ICollectionFixture<AlwaysAuthenticatedTestContext>
    {
    }
}