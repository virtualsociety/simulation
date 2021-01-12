using Xunit;
using Vs.Simulation.Shared;
using Vs.Simulation.Core.Probabilities;

namespace Vs.Simulation.Tests.Probabilities
{
    /// <summary>
    /// Next to Assertion, unit tests will also generate plots, for visual inspection.
    /// </summary>
    public class AgeTests
    {
        static AgeTests()
        {
            AgeProbability.Init();
        }

        [Theory]
        [InlineData(0.1,2020,2025, Constants.idx_gender_male)]
        [InlineData(0.1,2020,2025, Constants.idx_gender_female)]
        public void DefaultTest(float scale, int startYear, int endYear, int gender)
        {
            // Arrange
            var env = new SimSharp.ThreadSafeSimulation(42);
            for (int y = startYear; y <= endYear; y++)
            {
                var index = AgeProbability.YearIndex(y);
                Helpers.Sample(this, scale, y.ToString(), gender,
                    AgeProbability.Source[gender, index],
                    AgeProbability.Weights[gender, index],
                    "Age distribution"
                );
            }
        }
    }
}
