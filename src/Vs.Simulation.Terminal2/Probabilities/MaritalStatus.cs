using Deedle;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Vs.Simulation.Core.Probabilities
{
    /// <summary>
    /// Marriage probabilities, equal weighted distribution for testing purposes, needs statistical analysis for better probability modelling
    /// </summary>
    public static class MaritalStatus
    {
       //public static double Singles { get; set; }
       //public static double Married { get; set; }
       //public static double Partnerships { get; set; }



        /// <summary>
        /// Weight distribution of the probability
        /// </summary>
        public static List<double> Weights { get; set; } = new List<double> { 1, 1, 1 };
        /// <summary>
        /// A list of sources to select a sample from
        /// </summary>
        public static List<PartnerType> Source { get; private set; } = new List<PartnerType> { PartnerType.Single, PartnerType.Married, PartnerType.Partnership };

        private static Frame<int, string> MaritalAgeData;
        public static IList<double> SourceMaritalAge;
        public static IList<double> FemaleWeightsMaritalAge;
        public static IList<double> MaleWeightsMaritalAge;

        private static Frame<int, string>MaritalStatusData;
        public static IList<double> WeightsSingles;
        public static IList<double> WeightsMarried;
        public static IList<double> WeightsPartnership;


        static MaritalStatus()
        {
            MaritalAgeData = Frame.ReadCsv("../../../../../doc/data/NewMaritalAgeWeights.csv");
            FemaleWeightsMaritalAge = MaritalAgeData.GetColumn<double>("MarriedWomen").Values.Select(c => Convert.ToDouble(c)).ToList();
            MaleWeightsMaritalAge = MaritalAgeData.GetColumn<double>("MarriedMen").Values.Select(c => Convert.ToDouble(c)).ToList();
            SourceMaritalAge = MaritalAgeData.GetColumn<double>("MarriedWomen").Keys.Select(c => Convert.ToDouble(c)).ToList();


            MaritalStatusData = Frame.ReadCsv("../../../../../doc/data/marital_status.csv");
            WeightsSingles = MaritalStatusData.GetColumn<double>("Ongehuwden").Values.Select(c => Convert.ToDouble(c)).ToList();
            WeightsMarried = MaritalStatusData.GetColumn<double>("Gehuwden").Values.Select(c => Convert.ToDouble(c)).ToList();
            WeightsPartnership = MaritalStatusData.GetColumn<double>("Partnerschappen").Values.Select(c => Convert.ToDouble(c)).ToList();
        }

    }

    //MaritalStatus probabilities, for now equally distributed. Weights are being changed in the tests.
    //The difference between maritalStatus and Marital Status Age is that one provides partnership as chance in general.
    //Where Marital Status Age provides those statuses with Age as the main variable.
    public static class MaritalStatusAge
    {
        
        // Weight distribution of the probability
        public static List<double> Weights { get; set; } = new List<double> { 1, 1, 1 };
        /// A list of sources to select a sample from
        
        public static List<PartnerTypeAge> Source { get; private set; } = new List<PartnerTypeAge> { PartnerTypeAge.Single, PartnerTypeAge.Married, PartnerTypeAge.Divorced };


    }
    //PartnertypeAge is for MaritalStatusAge. Divorce is here included as there are no weights for partnerships.
    public enum PartnerTypeAge
    {
        Single,
        Married,
        Divorced
    }

    public enum PartnerType
    {
        Single,
        Married,
        Partnership,

    }
}
