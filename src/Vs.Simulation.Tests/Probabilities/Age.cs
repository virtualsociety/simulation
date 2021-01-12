using OxyPlot;
using Xunit;
using System.Collections.Generic;
using OxyPlot.Series;
using Vs.Simulation.Shared;
using Vs.Simulation.Core.Probabilities;
using OxyPlot.Axes;

namespace Vs.Simulation.Tests.Probabilities
{
    /// <summary>
    /// Next to Assertion, unit tests will also generate plots, for visual inspection.
    /// </summary>
    public class Age
    {
        static Age()
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
                var length = AgeProbability.Source[gender, index].Count;
                var cbsPoints = new List<DataPoint>(length);
                // var scale = 0.1; // 10%
                double simulatedPopulationSize = 0;
                int actualPopulationSize = 0;
                for (int i = 0; i < length; i++)
                {
                    actualPopulationSize += (int)AgeProbability.Weights[gender, index][i];
                    simulatedPopulationSize += AgeProbability.Weights[gender, index][i] * scale;
                    cbsPoints.Add(new DataPoint(i, (int)(AgeProbability.Weights[gender, index][i] * scale)));
                }
                simulatedPopulationSize = (int)simulatedPopulationSize;
                // Act
                // Age categories are on the X-Axis number of people on the X-Axis
                var points = new List<DataPoint>((int)simulatedPopulationSize);
                int[] data = new int[AgeProbability.Source[gender, index].Count];
                for (int x = 0; x < simulatedPopulationSize; x++)
                {
                    data[(int)env.RandChoice(AgeProbability.Source[gender, index], AgeProbability.Weights[gender, index])]++;
                }
                for (int j = 0; j < data.Length; j++)
                {
                    points.Add(new DataPoint(j, data[j]));
                }
                // Assert (through visual inspection)
                var model = new PlotModel { Title = $"Age distribution {y} - {actualPopulationSize} citizens {Constants.DisplayNames[gender]} scale {100 * scale}% " };
                model.DefaultColors = new List<OxyColor> { OxyColors.Red, OxyColors.Blue };
                model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "age" });
                model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "population" });
                model.Series.Add(new LineSeries { ItemsSource = points });
                model.Series.Add(new LineSeries { ItemsSource = cbsPoints });
                model.Series[0].Title = "simulated";
                model.Series[1].Title = "actual";
                Helpers.SvgWriter(model, this, $"{y}-{actualPopulationSize}-{Constants.DisplayNames[gender]}-scale-{scale}", new[]{ points, cbsPoints });
            }
        }
    }
}
