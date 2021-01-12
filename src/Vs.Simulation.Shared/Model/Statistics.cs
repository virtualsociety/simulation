namespace Vs.Simulation.Shared.Model
{
    public class Statistics
    {
        public long[] People { get; set; } = new long[2];
        public long Children { get; set; }
        public long ReachMaturity { get; set; }
        public long Couples { get; set; }
        public long Parents { get; set; }
        public long StackErrors { get; set; }

        public long Remarried { get; set; }
        public double AvgAgeMale
        {
            get
            {
                if (People[Constants.idx_gender_male] > 0)
                    return Global._totalAge[Constants.idx_gender_male] / (double)People[Constants.idx_gender_male];
                else
                    return 0;
            }
        }
        public double AvgAgeFemale
        {
            get
            {
                if (People[Constants.idx_gender_female] > 0)
                    return Global._totalAge[Constants.idx_gender_female] / (double)People[Constants.idx_gender_female];
                else
                    return 0;
            }
        }

        public int Deaths { get; set; }
    }
}
