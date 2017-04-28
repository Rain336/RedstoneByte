using Xunit;

namespace RedstoneByte.Test
{
    [CollectionDefinition("TestFile")]
    public class TestFileCollection : ICollectionFixture<TestFileFixture>
    {
    }
}