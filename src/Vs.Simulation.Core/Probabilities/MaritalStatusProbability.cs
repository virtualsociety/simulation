﻿using Deedle;
using System;
using System.Collections.Generic;
using System.Linq;
using Vs.Simulation.Shared;

namespace Vs.Simulation.Core.Probabilities
{
    /// <summary>
    /// Marriage probabilities, equal weighted distribution for testing purposes, needs statistical analysis for better probability modeling
    /// </summary>
    public static class MaritalStatusProbability
    {
        private static Frame<int, string> MaritalAgeMaleData;
        private static Frame<int, string> MaritalAgeFemaleData;
        public static List<double>[,] MaritalAgeWeights;
        public static List<double> MaritalAgeSource;


        private static Frame<int, string> MaritalStatusData;
        public static List<double>[] MaritalStatusWeights;
        public static List<int> MaritalStatusSource { get; set; } =
            new List<int> { Constants.marital_single, Constants.marital_married, Constants.marital_partner };

        private static Frame<int, string> RemarriageData;
        public static List<double>[,] RemarriageWeights;
        public static List<double> RemarriageSource { get; set; } = new List<double> { 0, 1 };

        public static int StartAge { get; private set; }
        public static int EndAge { get; private set; }

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
            StartAge = 18;
            EndAge = 100;
            MaritalAgeMaleData = Frame.ReadCsv("../../../../../doc/data/marital_age_male.csv");
            MaritalAgeFemaleData = Frame.ReadCsv("../../../../../doc/data/marital_age_female.csv");

            MaritalAgeWeights = new List<double>[2, EndYear - StartYear];
            MaritalAgeSource = MaritalAgeMaleData.GetColumn<double>($"{1950}").Keys.Select(c => Convert.ToDouble(c)).ToList();
            

            StartYear = 1950;
            EndYear = 2020;
            MaritalStatusWeights = new List<double>[EndYear - StartYear];
            MaritalStatusData = Frame.ReadCsv("../../../../../doc/data/marital_status.csv");
            var singles = MaritalStatusData.GetColumn<double>("Ongehuwden").Values.Select(c => Convert.ToDouble(c)).ToList();
            var married = MaritalStatusData.GetColumn<double>("Gehuwden").Values.Select(c => Convert.ToDouble(c)).ToList();
            var partnership = MaritalStatusData.GetColumn<double>("Partnerschappen").Values.Select(c => Convert.ToDouble(c)).ToList();

            RemarriageData = Frame.ReadCsv("../../../../../doc/data/remarriage_weights.csv");
            RemarriageWeights = new List<double>[2, EndYear - StartYear];

            for (int i = 0; i < EndYear - StartYear; i++)
            {
                MaritalAgeWeights[Constants.idx_gender_male, i] = MaritalAgeMaleData.GetColumn<double>($"{i + StartYear}").Values.Select(c => Convert.ToDouble(c)).ToList();
                MaritalAgeWeights[Constants.idx_gender_female, i] = MaritalAgeFemaleData.GetColumn<double>($"{i + StartYear}").Values.Select(c => Convert.ToDouble(c)).ToList();

                MaritalStatusWeights[i] = new List<double>() { singles[i], married[i], partnership[i] };

                var remarriage = RemarriageData.GetColumn<double>($"{i + StartYear}").Values.Select(c => Convert.ToDouble(c)).ToList();
                RemarriageWeights[Constants.idx_gender_male, i] = new List<double>() { remarriage[0], remarriage[1] };
                RemarriageWeights[Constants.idx_gender_female, i] = new List<double>() { remarriage[2], remarriage[3] };
            }
        }
    }
}
