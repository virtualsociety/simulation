using System;
using System.Collections.Generic;
using System.Text;
using Deedle;

namespace Vs.Simulation.Core.Probabilities
{
    public class MaritalDuration
    {
        public static IList<double> DurationSource = new List<double>
        {
            1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21
        };
        public static IList<double> DurationWeights = new List<double>
        {
            56958,
            57928,
            58861,
            59882,
            61021,
            62306,
            63547,
            64849,
            66125,
            67445,
            68675,
            70258,
            71995,
            73884,
            75886,
            77895,
            79887,
            82004,
            83971,
            85386,
            85776,


        };

        static MaritalDuration()
        {
           

        }

    }

    
}
