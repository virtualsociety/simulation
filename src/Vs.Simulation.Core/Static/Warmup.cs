using Microsoft.FSharp.Control;
using SimSharp;
using System.Collections.Generic;
using Vs.Simulation.Core.Probabilities;
using Vs.Simulation.Shared;

namespace Vs.Simulation.Core
{
    /// <summary>
    /// Population Warm up process, creates a population, without parents
    /// 
    /// For perpetual growth in the actual simulation, we will generate the following
    /// 
    /// LifeSpan
    /// Number Of Children
    /// 
    /// Based on initial statistics (not by DES event scedule)
    /// 
    /// </summary>
    public partial class Population
    {
        /// <summary>
        /// Warms up a society by scale. i.e. 0.1 will start a population on a 10% scale.
        /// </summary>
        /// <param name="scale"></param>
        public void Warmup(float scale)
        {
            // Recreate Total population by year counting number of people per age group.
            var index = AgeProbability.YearIndex(Environment.StartDate.Year);
            // Traverse all age categories for males and females.
            for (int age = 0;age<AgeProbability.Source[0,0].Count; age ++)
            {
                var people = (int)(AgeProbability.Weights[Constants.idx_gender_male, index][age] * scale);
                for (int j =0; j < people; j++)
                {
                    new Person(Environment, age);
                }
            }
        }
    }
}
