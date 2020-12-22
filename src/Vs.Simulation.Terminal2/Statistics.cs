namespace Vs.Simulation.Terminal2
{
    public static class Statistics
    {
        public static long[] People { get; set; } = new long[2];
        public static long Children { get; set; }
        public static long ReachMaturity { get; set; }
        public static long Couples { get; set; }
        public static long Parents { get; set; }
        public static long StackErrors { get; set; }
        public static double AvgAgeMale { get {
                if (People[Constants.idx_gender_male] > 0)
                    return ((double)Global._totalAge[Constants.idx_gender_male] / (double)People[Constants.idx_gender_male]);
                else
                    return 0;
            } 
        }
        public static double AvgAgeFemale { 
            get {
                if (People[Constants.idx_gender_female] > 0)
                    return ((double)Global._totalAge[Constants.idx_gender_female] / (double)People[Constants.idx_gender_female]);
                else
                    return 0;
            }
        }

        public static int Deaths { get; set; }
    }
}
