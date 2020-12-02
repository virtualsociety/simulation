using System.Collections.Generic;

namespace Vs.Simulation.Core.Probabilities
{
    /// <summary>
    /// Marriage probabilities, equal weighted distribution for testing purposes, needs statistical analysis for better probability modelling
    /// </summary>
    public static class MaritalStatus
    {
        /// <summary>
        /// Weight distribution of the probability
        /// </summary>
        public static List<double> Weights { get; set; } = new List<double> { 1, 1, 1 };
        /// <summary>
        /// A list of sources to select a sample from
        /// </summary>
        public static List<PartnerType> Source { get; private set; } = new List<PartnerType> { PartnerType.Single, PartnerType.Married, PartnerType.Partnership };

        
    }

    //MaritalStatus propabilities, for now equally distributed. Weights are being changed in the tests.
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
