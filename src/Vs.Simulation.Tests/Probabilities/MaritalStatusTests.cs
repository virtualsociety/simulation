using Vs.Simulation.Core.Probabilities;
using Xunit;
using Vs.Simulation.Shared;
using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

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
            var env = new SimSharp.ThreadSafeSimulation(42);
            var length = MaritalStatusProbability.MaritalAgeSource.Count;
            var total = 0;
            string y = "0000"; //  no year data....
            var simulatedTotal = 0;
            var cbsPoints = new List<DataPoint>(length);
            for (int i =0;i<length; i++)
            {
                total += (int)MaritalStatusProbability.MaritalAgeWeights[gender][i];
                simulatedTotal += (int)(MaritalStatusProbability.MaritalAgeWeights[gender][i] * scale);
                cbsPoints.Add(new DataPoint(i, (int)(MaritalStatusProbability.MaritalAgeWeights[gender][i] * scale)));
            }
            int[] data = new int[length];
            // Sample some probability data, based on scale of total.
            for (var s=0;s<total*scale;s++)
            {
                data[(int)env.RandChoice(MaritalStatusProbability.MaritalAgeSource,
                    MaritalStatusProbability.MaritalAgeWeights[gender])]++;
            }
            var points = new List<DataPoint>(length);
            for (int j = 0; j < data.Length; j++)
            {
                points.Add(new DataPoint(j, data[j]));
            }
            // Assert (through visual inspection)
            var model = new PlotModel { Title = $"Marital Age distribution - {total} citizens {Constants.DisplayNames[gender]} scale {100 * scale}% " };
            model.DefaultColors = new List<OxyColor> { OxyColors.Red, OxyColors.Blue };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "age" });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "population" });
            model.Series.Add(new LineSeries { ItemsSource = points });
            model.Series.Add(new LineSeries { ItemsSource = cbsPoints });
            model.Series[0].Title = "simulated";
            model.Series[1].Title = "actual";
            Helpers.SvgWriter(model, this, $"{y}-{total}-{Constants.DisplayNames[gender]}-scale-{scale}", new[] { points, cbsPoints });
        }
    }
}
