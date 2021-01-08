using System;
using System.Collections.Generic;
using System.Text;
using Deedle;
using System.Linq;

namespace Vs.Simulation.Core.Probabilities
{
    public static class Children
    {
        private static Frame<int, string> MotherData;
        public static IList<double>[] MotherWeights;
        public static List<double> MotherChildSource { get; set; } = new List<double> { 0, 1 };

        //These are the weights for the amount of children a women can have
        private static Frame<int, string> ChildData;
        public static List<double>[] ChildAmountWeights;
        public static List<double> SourceAmountChildren { get; set; } = new List<double> { 1, 2, 3, 4 };

        private static Frame<int, string> LabourData;
        public static List<double>[] LabourYears;

        public static int StartAge { get; private set; }
        public static int EndAge { get; private set; }

        public static int StartYear { get; private set; }
        public static int EndYear { get; private set; }


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

            StartYear = 1950;
            EndYear = 2020;
            ChildAmountWeights = new List<double>[EndYear - StartYear];
            ChildData = Frame.ReadCsv("../../../../../doc/data/motherAmountChildren.csv");

            LabourData = Frame.ReadCsv("../../../../../doc/data/motherlabour_ages.csv");
            LabourYears = new List<double>[EndYear - StartYear];

            for (int i = 0; i < EndYear - StartYear; i++)
            {
                var childAmount = ChildData.GetColumn<double>($"{i + StartYear}").Values.Select(c => Convert.ToDouble(c)).ToList();
                ChildAmountWeights[i] = new List<double>() { childAmount[0], childAmount[1], childAmount[2], childAmount[3] };
            }

            for (int i = 0; i < EndYear - StartYear; i++)
            {
                var labourYears = LabourData.GetColumn<double>($"{i + StartYear}").Values.Select(c => Convert.ToDouble(c)).ToList();
                LabourYears[i] = new List<double>() { labourYears[0], labourYears[1], labourYears[2] };
            }
        }
    }
}
