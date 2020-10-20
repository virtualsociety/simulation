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

    public enum PartnerType
    {
        Single,
        Married,
        Partnership
    }
}
