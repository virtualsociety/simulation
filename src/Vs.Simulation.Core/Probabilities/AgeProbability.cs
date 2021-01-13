using Deedle;
using System;
using System.Collections.Generic;
using System.Linq;
using Vs.Simulation.Shared;

namespace Vs.Simulation.Core.Probabilities
{
    /// <summary>
    /// Age probability weighted list distributions per year, for males and females.
    /// </summary>
    public static class AgeProbability
    {
        /// <summary>
        /// Data frames for reading csv, where int indicates the gender.
        /// </summary>
        private static Frame<int, string>[] AgeData;
        /// <summary>
        /// Compiled list for usage with SimSharp weight distribution choices (contains number of people)
        /// </summary>
        public static IList<double>[,] Weights;
        /// <summary>
        /// Compiled list for usage with SimSharp weight distribution choices (contains age categories)
        /// </summary>
        public static IList<double>[,] Source;
        /// <summary>
        /// Start Year of the data set
        /// </summary>
        public static int StartYear { get; private set; }
        /// <summary>
        /// End Year of the data set
        /// </summary>
        public static int EndYear { get; private set; }
        /// <summary>
        /// (re)initialize the dataset
        /// </summary>
        
        public static int YearIndex(int year)
        {
            return year - StartYear;
        }


        public static void Init()
        {
            StartYear = 1950;
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
