using Vs.Simulation.Core.Probabilities;
using Xunit;
using Vs.Simulation.Shared;

namespace Vs.Simulation.Tests.Probabilities
{
    public class MaritalDurationTests
    {
        [Theory]
        [InlineData(0.1, 0, 0)]
        // TODO: This probability data set does not contain years. Year not specified.
        public void MaritalDuration(float scale, int startDate, int endDate)
        {
            Helpers.Sample(this, scale, "0000", Constants.idx_empty,
                MaritalDurationProbability.Source,
                MaritalDurationProbability.Weights,
                "Marital Age distribution","duration"
            );
        }
    }
}
