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
        [InlineData(0.1, 1950, 1950, Constants.idx_gender_male)]
        [InlineData(0.1, 1950, 1950, Constants.idx_gender_female)]
        [InlineData(0.1, 2021, 2021, Constants.idx_gender_male)]
        [InlineData(0.1, 2021, 2021, Constants.idx_gender_female)]
        [InlineData(0.1, 2040, 2040, Constants.idx_gender_male)]
        [InlineData(0.1, 2040, 2040, Constants.idx_gender_female)]
        public void AgeDistribution(float scale, int startYear, int endYear, int gender)
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
