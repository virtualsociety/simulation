using OxyPlot;
using System.IO;
using System.Runtime.CompilerServices;

namespace Vs.Simulation.Tests
{
    public static class Helpers
    {
        public static void SvgWriter(PlotModel model, object caller, string suffix, [CallerMemberName] string callerName = "")
        {
            var directory = caller.GetType().Name;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            var image = new SvgExporter { Width = 600, Height = 400 };
            File.WriteAllText($"./{directory}/{callerName}-{suffix}.svg", image.ExportToString(model));
        }
    }
}
