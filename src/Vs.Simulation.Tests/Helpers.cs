using Deedle;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Vs.Simulation.Shared;

namespace Vs.Simulation.Tests
{
    public static class Helpers
    {
        public static void SvgWriter(PlotModel model, object caller, string suffix, IEnumerable<DataPoint>[] pointData, [CallerMemberName] string callerName = "")
        {
            var directory = caller.GetType().Name;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            var image = new SvgExporter { Width = 600, Height = 400 };
            var name = $"./{directory}/{callerName}-{suffix}";
            File.WriteAllText($"{name}.svg", image.ExportToString(model));

            // Save point data as csv
            for (int i=0; i<model.Series.Count;i++)
            {
                LineSeries serie = (LineSeries)model.Series[i];
                Frame.FromRecords(pointData[i]).SaveCsv($"{name}-{serie.Title}.csv");
            }
        }

        public static void Sample(object caller, float scale, string year, int gender, 
            IList<double> source, IList<double> weights, string title, string xlabel = "age", string ylabel = "population", [CallerMemberName] string callerName = "")
        {
            var env = new SimSharp.ThreadSafeSimulation(42);
            var length = source.Count;
            var total = 0;
            var simulatedTotal = 0;
            var cbsPoints = new List<DataPoint>(length);
            for (int i = 0; i < length; i++)
            {
                total += (int)weights[i];
                simulatedTotal += (int)(weights[i] * scale);
                cbsPoints.Add(new DataPoint(i, (int)(weights[i] * scale)));
            }
            int[] data = new int[length];

            // Sample some probability data, based on scale of total.
            for (var s = 0; s < total * scale; s++)
            {
                data[(int)env.RandChoice(source,
                    weights)]++;
            }
            var points = new List<DataPoint>(length);
            for (int j = 0; j < data.Length; j++)
            {
                points.Add(new DataPoint(j, data[j]));
            }
            // Assert (through visual inspection)
            var model = new PlotModel { Title = $"{title} {year} - {total} citizens {Constants.DisplayNames[gender]} scale {100 * scale}% " };
            model.DefaultColors = new List<OxyColor> { OxyColors.Silver, OxyColors.Black };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = xlabel });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = ylabel });
            model.Series.Add(new LineSeries { ItemsSource = points, StrokeThickness= 4 });
            model.Series.Add(new LineSeries { ItemsSource = cbsPoints, StrokeThickness = 1 });
            model.Series[0].Title = "simulated";
            model.Series[1].Title = "actual";
            Helpers.SvgWriter(model, caller, $"{year}-{total}-{Constants.DisplayNames[gender]}-scale-{scale}", new[] { points, cbsPoints }, callerName);
        }

    }
}
