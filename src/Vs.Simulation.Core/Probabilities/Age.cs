using Deedle;
using System;
using System.Collections.Generic;
using System.Linq;
using Vs.Simulation.Shared;

namespace Vs.Simulation.Core.Probabilities
{
    public static class Age
    {
        private static Frame<int, string>[] AgeData;
        public static IList<double>[,] Weights;
        public static IList<double>[,] Source;

        public static int StartYear { get; private set; }
        public static int EndYear { get; private set; }

        public static void Init()
        {
            StartYear = 2020;
            EndYear = 2060;
            Weights = new IList<double>[2, EndYear - StartYear];
            Source = new IList<double>[2, EndYear - StartYear];
            AgeData = new Frame<int, string>[2];

            for (int i = 0; i < EndYear - StartYear; i++)
            {
                // https://www.cbs.nl/nl-nl/visualisaties/bevolkingspiramide
                AgeData[Constants.idx_gender_male] = Frame.ReadCsv("../../../../../doc/data/ages_male.csv");
                AgeData[Constants.idx_gender_female] = Frame.ReadCsv("../../../../../doc/data/ages_female.csv");

                Weights[Constants.idx_gender_male, i] = AgeData[Constants.idx_gender_male]
                    .GetColumn<double>($"{i + StartYear}").Values.Select(c => Convert.ToDouble(c)).ToList();
                Source[Constants.idx_gender_male, i] = AgeData[Constants.idx_gender_male]
                    .GetColumn<double>($"{i + StartYear}").Keys.Select(c => Convert.ToDouble(c)).ToList();

                Weights[Constants.idx_gender_female, i] = AgeData[Constants.idx_gender_female]
                    .GetColumn<double>($"{i + StartYear}").Values.Select(c => Convert.ToDouble(c)).ToList();
                Source[Constants.idx_gender_female, i] = AgeData[Constants.idx_gender_female]
                    .GetColumn<double>($"{i + StartYear}").Keys.Select(c => Convert.ToDouble(c)).ToList();
            }
        }
    }
}
