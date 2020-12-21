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


        public static int StartAge { get; private set; }
        public static int EndAge { get; private set; }

        public static void Init() 
        {
            StartAge = 18;
            EndAge = 49;
            MotherWeights = new List<double>[EndAge - StartAge];
            MotherData = Frame.ReadCsv("../../../../../doc/data/motherbirthing_ages.csv");
            var noChildren = MotherData.GetColumn<double>("No Children").Values.Select(c => Convert.ToDouble(c)).ToList();
            var children = MotherData.GetColumn<double>("Children").Values.Select(c => Convert.ToDouble(c)).ToList(); ;

            for (int i = 0; i < EndAge - StartAge; i++) 
            {
                MotherWeights[i] = new List<double>() { noChildren[i], children[i] };
                
            }
        }

       // //Weights are set 1:1 because it changes per age range
       // public static List<double> WeightGetChildren { get; set; } = new List<double> { 1, 1 };
       // public static List<double> SourceGetChildren { get; set; } = new List<double> { 0, 1 };
       //
       // //These are the weights for the amount of children a women can have
       // public static List<double> WeightsAmountChildren { get; set; } = new List<double> { 567014, 710251, 304442};
       // public static List<double> SourceAmountChildren { get; set; } = new List<double> { 1, 2, 3 };
       //
       // //These weights are for the mothers age during birth
       // public static IList<double> AgeBirthMotherSource;
       // public static IList<double> AgeBirthMotherWeights;
       // public static IList<double> AgeNotABirthMothersWeights;
       // private static Frame<int, string> MotherData;
       
       //static Children()
       //{
       //    MotherData = Frame.ReadCsv("../../../../../doc/data/motherbirthing_ages.csv");
       //    AgeBirthMotherWeights = MotherData.GetColumn<double>("Children").Values.Select(c => Convert.ToDouble(c)).ToList();
       //    AgeBirthMotherSource = MotherData.GetColumn<double>("Children").Keys.Select(c => Convert.ToDouble(c)).ToList();
       //    AgeNotABirthMothersWeights = MotherData.GetColumn<double>("No Children").Values.Select(c => Convert.ToDouble(c)).ToList();
       //}
    }

    
}
