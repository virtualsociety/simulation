using Deedle;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Vs.Simulation.Core.Probabilities
{
    public static class Age
    {
        private static Frame<int, string> AgeDataMale;
        public static IList<double> MaleWeights;
        public static IList<double> MaleSource;

        private static Frame<int, string> AgeDataFemale;
        public static IList<double> FemaleWeights;
        public static IList<double> FemaleSource;

        static Age()
        {
            // Determine lifespan, TODO: only 2020 is used for weights
            // No estimates are followed in the 2021-2060 prognosis yet
            // Compiled from prognosis until the year 2060 
            // https://www.cbs.nl/nl-nl/visualisaties/bevolkingspiramide
            AgeDataMale = Frame.ReadCsv("../../../../../doc/data/ages_male.csv");
            MaleWeights = AgeDataMale.GetColumn<double>("2020").Values.Select(c => Convert.ToDouble(c)).ToList();
            MaleSource = AgeDataMale.GetColumn<double>("2020").Keys.Select(c => Convert.ToDouble(c)).ToList();
            AgeDataFemale = Frame.ReadCsv("../../../../../doc/data/ages_female.csv");
            FemaleWeights = AgeDataFemale.GetColumn<double>("2020").Values.Select(c => Convert.ToDouble(c)).ToList();
            FemaleSource = AgeDataFemale.GetColumn<double>("2020").Keys.Select(c => Convert.ToDouble(c)).ToList();
        }
    }
}
