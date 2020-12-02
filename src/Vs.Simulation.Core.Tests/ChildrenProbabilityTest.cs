using Deedle;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Vs.Simulation.Core.Probabilities;
using System.Linq;

namespace Vs.Simulation.Core.Tests
{

    public class ChildrenProbability 
    {
        public int Age { get; set; }

        public  int[][][] childrenAmount { get; set; }
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
        [Fact]
        public static void ChildrenProbabilities() 
        {
            //Arrange
            var env = new SimSharp.ThreadSafeSimulation(42);
            Frame<int, string> frame;
            frame = Frame.ReadCsv("../../../../../doc/data/maritalAgeProbability.csv");
            var womenAges = frame.GetColumn<double>("MarriedWomen").Values.Select(c => Convert.ToDouble(c)).ToList();
            Frame<int, string> MotherData = Frame.ReadCsv("../../../../../doc/data/motherbirthing_ages.csv");
            List<double> weights;
            List<double> childChoice;
            var childAmount = new Dictionary<int, List<double>>();

            var collection = new ChildrenProbabilityCollection();
            var collectionNew = new ChildrenProbabilityCollection();
            
            //Act
            //Here the mothers are randomly assigned to a designated amount of children.
            //The reults are first put in a list and after that they get sorted.
            for (int i = 0; i < 32; i++) 
            {
                childAmount.Add(i, new List<double>());
                childChoice = new List<double>();
                int age = i + 18;
                for (int j = 0; j < womenAges[i]; j++)
                {
                    weights = MotherData.GetColumn<double>(Convert.ToString(age)).Values.Select(c => Convert.ToDouble(c)).ToList();
                    Children.WeightGetChildren = weights;
                    childChoice.Add(env.RandChoice(Children.SourceGetChildren, Children.WeightGetChildren));
                }

                var children = childChoice.Where(p => p == 1).Count();
                //childAmount = new List<double>();

                for (int j = 0; j < children; j++)
                {
                    childAmount[i].Add(env.RandChoice(Children.SourceAmountChildren, Children.WeightsAmountChildren));
                }

                //Here they are getting sorted and added to a new collection
                collectionNew.Add(new ChildrenProbability()
                {
                    Age = age,
                    childrenAmount = { }
                    //WomenNoChildren = childChoice.Where(p => p == 0).Count(),
                    //MotherOfOneAgeFirstBorn = childAmount[i].Where(p => p == 1).Count(),
                    //MotherOfTwoAgeFirtBorn = childAmount[i].Where(p => p == 2).Count(),
                    //MotherOfThreeAgeFirstBorn = childAmount[i].Where(p => p == 3).Count(),
                    //BirthingMothersTotal = children
                }) ;
                
            }

            //Both collections get exported out
            var exportNew = Frame.FromRecords(collectionNew);
            var export = Frame.FromRecords(collection);
            exportNew.SaveCsv("../../../../../doc/data/childrenProbabilityDifferent.csv");

            //Assert
            int expectedFirstChildren34 = 2564;
            int actualFirstChildren34 = childAmount[16].Where(p => p == 1).Count();
            Assert.Equal(expectedFirstChildren34, actualFirstChildren34);

            int expectedSecondChildren18 = 6;
            int actualSecondChildren18 = childAmount[0].Where(p => p == 2).Count();
            Assert.Equal(expectedSecondChildren18, actualSecondChildren18);

            int expectedThirdChildren43 = 73;
            int actualThirdChildren43 = childAmount[25].Where(p => p == 3).Count();
            Assert.Equal(expectedThirdChildren43, actualThirdChildren43);
        }
    }
}
