using System;
using System.Collections.Generic;
using System.Text;
using Deedle;
using System.Linq;

namespace Vs.Simulation.Core.Probabilities
{
    public static class Children
    {
        public static List<double> WeightGetChildren { get; set; } = new List<double> { 1, 1 };
        public static List<double> SourceGetChildren { get; set; } = new List<double> { 0, 1 };

        //These are the weights for the amount of children a women can habe
        public static List<double> WeightsAmountChildren { get; set; } = new List<double> { 567014, 710251, 304442};
        public static List<double> SourceAmountChildren { get; set; } = new List<double> { 1, 2, 3 };

        //These weights are for the mothers age during birth
        public static IList<double> WeightsMotherBirthingAge;
        public static IList<double> SourceMotherBirthingAge;
        private static Frame<int, string> MotherData;
        static Children()
        {
            //MotherData = Frame.ReadCsv("../../../../../doc/data/motherbirthing_ages.csv");
            //WeightsMotherBirthingAge = MotherData.GetColumn<double>("2019").Values.Select(c => Convert.ToDouble(c)).ToList();
            //SourceMotherBirthingAge = MotherData.GetColumn<double>("2019").Keys.Select(c => Convert.ToDouble(c)).ToList();
        }
    }

    
}
