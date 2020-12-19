using System.Collections.Generic;

namespace Vs.Simulation.Terminal2.Probabilities
{
    public static class Gender
    {
        /// <summary>
        /// Weight distribution of the probability
        /// </summary>
        public static List<double> Weights { get; private set; } = new List<double> { 127, 100 };
        /// <summary>
        /// A list of sources to select a sample from
        /// </summary>
        public static List<bool> Source { get; private set; } = new List<bool> { Constants.gender_male, Constants.gender_female };
    }
}
