using Xunit;

[assembly: TestCaseOrderer("Xunit.Extensions.Ordering.TestCaseOrderer", "Xunit.Extensions.Ordering")]
[assembly: TestCollectionOrderer("Xunit.Extensions.Ordering.CollectionOrderer", "Xunit.Extensions.Ordering")]
namespace Vs.Simulation.Core.Tests
{
    public static class Global
    {
        public static string GetDataFolder()
        {
            return "../../../../../doc/data/unit-test/";
        }
    }
}
