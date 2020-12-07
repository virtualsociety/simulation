using System.Collections.Generic;

namespace Vs.Simulation.Core.Tests
{

    public class ChildrenProbability 
    {
        public int Age { get; set; }

        public  int[][][][] childrenAmount { get; set; }
        //public int WomenNoChildren { get; set; }
        //public int MotherOfOneAgeFirstBorn { get; set; }
        //public int MotherOfTwoAgeFirtBorn { get; set; }
        //public int MotherOfThreeAgeFirstBorn { get; set; }
        //
        //public int BirthingMothersTotal { get; set; }
    }

    public class ChildrenProbabilityCollection : List<ChildrenProbability> 
    { }
    public class ChildrenProbabilityTest
    {

    }
}
