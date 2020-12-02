using Deedle;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using Vs.Simulation.Core.Probabilities;

namespace Vs.Simulation.Core.Tests
{
    public class MaritalAgeProbability
    {
        public int Age { get; set; }
        public int SingleWomen { get; set; }
        public int MarriedWomen { get; set; }
        public int DivorcedWomen { get; set; }
        public int SingleMen { get; set; }
        public int MarriedMen { get; set; }
        public int DivorcedMen { get; set; }
    }

    public class MaritalAgePropabilityCollection : List<MaritalAgeProbability>
    { }

    public class MaritalAgeProbabilityTest
    {   
        [Fact]
        public static void MaritalAgePropability1() 
        {
            //Arrange
            var env = new SimSharp.ThreadSafeSimulation(42);
            Frame<int, string> frame;
            frame = Frame.ReadCsv("../../../../../doc/data/MaritalAgeWeights.csv");
            List<double> weights;
            var womenList = new Dictionary<int,List<PartnerTypeAge>>();
            var menList = new Dictionary<int, List<PartnerTypeAge>>();
            var collection = new MaritalAgePropabilityCollection();
            var femaleAge = Age.FemaleWeights;
            var maleAge = Age.MaleWeights;

            //Act
            //In here we decide what marital status someone has at a certain age.
            //In our case it goes from 18 to 99 years old.
            for (int i = 18; i < 100; i++) 
            {
                //We made a dictionary for both men and women.
                womenList.Add(i, new List<PartnerTypeAge>());
                menList.Add(i, new List<PartnerTypeAge>());

                //Female age is all the predictated ages of females in 2020.
                //This can be found in the probability class "Age".
                //First they get the weights from their age group. And with those weights deciding the results in a randomchoice, they get put into their dictionary.
                for (int j = 0; j < femaleAge[i]; j++) 
                {
                    weights = frame.GetColumn<double>(Convert.ToString(i)).Values.Select(c => Convert.ToDouble(c)).Skip(3).Take(3).ToList();
                    MaritalStatusAge.Weights = weights;
                    womenList[i].Add(env.RandChoice(MaritalStatusAge.Source, MaritalStatusAge.Weights));
                }

                //The exact same happens as stated above in women.
                for (int j = 0; j < maleAge[i]; j++) 
                {
                    weights = frame.GetColumn<double>(Convert.ToString(i)).Values.Select(c => Convert.ToDouble(c)).Take(3).ToList();
                    MaritalStatusAge.Weights = weights;
                    menList[i].Add(env.RandChoice(MaritalStatusAge.Source, MaritalStatusAge.Weights));
                }
                
                //In here we sort everything into their respective classes. 
                //i here is yet again representing an age group.
                collection.Add(new MaritalAgeProbability()
                {
                    SingleWomen = womenList[i].Where(p => p == PartnerTypeAge.Single).Count(),
                    MarriedWomen = womenList[i].Where(p => p == PartnerTypeAge.Married).Count(),
                    DivorcedWomen = womenList[i].Where(p => p == PartnerTypeAge.Divorced).Count(),
                    SingleMen = menList[i].Where(p => p == PartnerTypeAge.Single).Count(),
                    MarriedMen = menList[i].Where(p => p == PartnerTypeAge.Married).Count(),
                    DivorcedMen = menList[i].Where(p => p == PartnerTypeAge.Divorced).Count(),
                    Age = i
                });
            }
            //Putting the collection into a CSV which can be found in the location stated below.
            var export = Frame.FromRecords(collection);
            export.SaveCsv("../../../../../doc/data/maritalAgeProbability.csv");


            //Assert
            //We asserted a few ages to see if the test would keep on the same results as before.
            //For women we used ages 18, 36 and 98 as for men we used 30, 57 and 67.
            int expectedSingleW = 42679;
            int actualSingleW = womenList[36].Where(p => p == PartnerTypeAge.Single).Count();

            int expectedMarriedW = 92;
            int actualMarriedW = womenList[18].Where(p => p == PartnerTypeAge.Married).Count();

            int expectedDivorcedW = 741;
            int actualDivorcedW = womenList[98].Where(p => p == PartnerTypeAge.Divorced).Count();

            int expectedSingleM = 9900;
            int actualSingleM = menList[67].Where(p => p == PartnerTypeAge.Single).Count();

            int expectedMarriedM = 81637;
            int actualMarriedM = menList[57].Where(p => p == PartnerTypeAge.Married).Count();

            int expectedDivorcedM = 1603;
            int actualDivorcedM = menList[30].Where(p => p == PartnerTypeAge.Divorced).Count();

            Assert.Equal(expectedSingleW, actualSingleW);
            Assert.Equal(expectedMarriedW, actualMarriedW);
            Assert.Equal(expectedDivorcedW, actualDivorcedW);

            Assert.Equal(expectedSingleM, actualSingleM);
            Assert.Equal(expectedMarriedM, actualMarriedM);
            Assert.Equal(expectedDivorcedM, actualDivorcedM);


        }
    }
}
