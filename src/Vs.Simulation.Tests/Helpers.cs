using Deedle;
using OxyPlot;
using OxyPlot.Series;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

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
    }
}
