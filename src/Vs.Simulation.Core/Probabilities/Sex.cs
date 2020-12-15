using System.Collections.Generic;

namespace Vs.Simulation.Core.Probabilities
{
    /// <summary>
    /// Women have pairs of two X chromosomes; males have pairs of an X and a Y chromosome.Textbooks 
    /// and scientific literature often cite a sex ratio at conception of 1.27, or 127 boys for every 100 girls.
    /// https://www.cbs.nl/nl-nl/visualisaties/bevolkingspiramide
    /// </summary>
    public static class Gender
    {
        /// <summary>
        /// Weight distribution of the probability
        /// </summary>
        public static List<double> Weights { get; private set; } = new List<double> { 127, 100 };
        /// <summary>
        /// A list of sources to select a sample from
        /// </summary>
        public static List<GenderType> Source { get; private set; } = new List<GenderType> { GenderType.Male, GenderType.Female };

    }

    /// <summary>
    /// Sex types
    /// </summary>
    public enum GenderType {
            Male,
            Female
    }
}
