using System.Collections.Generic;

namespace Vs.Simulation.Core.Probabilities
{
    public class MaritalDurationProbability
    {
        //In here you can find the weights for MaritalDuration. With duration we mean duration in years. So each source number represent a year.
        public static IList<double> Source = new List<double>
        {
            0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71
        };

        //In here the weights that go with the marital duration.
        public static IList<double> Weights = new List<double>
        {
            59648,
            62084,
            63039,
            62127,
            61997,
            59462,
            63380,
            62190,
            63875,
            63311,
            63428,
            59588,
            56650,
            55564,
            55973,
            58515,
            59830,
            57107,
            60569,
            59952,
            58317,
            56958,
            54985,
            53465,
            54139,
            55600,
            58024,
            56994,
            58075,
            55459,
            53998,
            53133,
            52157,
            48997,
            47326,
            46440,
            49048,
            50060,
            52464,
            48587,
            50090,
            51923,
            53335,
            54889,
            58428,
            55862,
            61043,
            61838,
            59989,
            55341,
            52939,
            49353,
            44697,
            40333,
            34862,
            29240,
            25510,
            22242,
            18229,
            15283,
            13145,
            10552,
            7981,
            5710,
            4095,
            2847,
            1982,
            1366,
            773,
            459,
            309,
            422,
        };
    }

}
