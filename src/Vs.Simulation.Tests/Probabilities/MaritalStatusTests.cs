using Vs.Simulation.Core.Probabilities;
using Xunit;
using Vs.Simulation.Shared;

namespace Vs.Simulation.Tests.Probabilities
{
    public class MaritalStatusTests
    {
        static MaritalStatusTests()
        {
            MaritalStatusProbability.Init();

        }

        [Theory]
        [InlineData(0.1, 0, 0, Constants.idx_gender_male)]
        [InlineData(0.1, 0, 0, Constants.idx_gender_female)]
        // TODO: This probability data set does not contain years. Year not specified.
        public void MaritalAge(float scale, int startDate, int endDate, int gender)
        {
            Helpers.Sample(this, scale, "0000", gender, 
                MaritalStatusProbability.MaritalAgeSource,
                MaritalStatusProbability.MaritalAgeWeights[gender], 
                "Marital Age distribution"
            );
        }
        /*
        [Theory]
        [InlineData(0.1, 2015, 2019, Constants.idx_gender_male)]
        [InlineData(0.1, 2015, 2019, Constants.idx_gender_female)]
        // TODO: This probability data should have a different representation.
        public void RemarriageAge(float scale, int startDate, int endDate, int gender)
        {
            for (int year = startDate; year <= endDate; year++)
            {
                Helpers.Sample(this, scale, year.ToString(), gender,
                    MaritalStatusProbability.RemarriageSource,
                    MaritalStatusProbability.RemarriageWeights[gender,MaritalStatusProbability.YearIndex(year)],
                    "Remarry Age distribution"
                );
            }
        }
        */
    }
}
