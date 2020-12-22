using System;
using System.Collections.Generic;
using System.Text;
using Deedle;
using System.Linq;

namespace Vs.Simulation.Terminal2.Probabilities
{
    public static class Children
    {
        private static Frame<int, string> MotherData;
        public static IList<double>[] MotherWeights;
        public static List<double> MotherChildSource { get; set; } = new List<double>{ 0, 1 };

        //These are the weights for the amount of children a women can have
        public static List<double> WeightsAmountChildren { get; set; } = new List<double> { 567014, 710251, 304442 };
        public static List<double> SourceAmountChildren { get; set; } = new List<double> { 1, 2, 3 };

        public static int StartAge { get; private set; }
        public static int EndAge { get; private set; }

        public static void Init() 
        {
            StartAge = 18;
            EndAge = 49;
            MotherWeights = new List<double>[EndAge - StartAge];
            MotherData = Frame.ReadCsv("../../../../../doc/data/motherbirthing_ages.csv");
            var noChildren = MotherData.GetColumn<double>("No Children").Values.Select(c => Convert.ToDouble(c)).ToList();
            var children = MotherData.GetColumn<double>("Children").Values.Select(c => Convert.ToDouble(c)).ToList();

            for (int i = 0; i < EndAge - StartAge; i++) 
            {
                MotherWeights[i] = new List<double>() { noChildren[i], children[i] };
                
            }
        }
    }  
}
