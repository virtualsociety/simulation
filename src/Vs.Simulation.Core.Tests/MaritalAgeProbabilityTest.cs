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


    //public class PropabilityCalculation 
    //{
    //
    //    public PropabilityCalculation(int env, Frame Frame, List<double> weights) 
    //    {
    //
    //
    //    }
    //}

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
            Random rnd = new Random(42);
            var womenList = new Dictionary<int,List<PartnerTypeAge>>();
            var menList = new Dictionary<int, List<PartnerTypeAge>>();
            var collection = new MaritalAgePropabilityCollection();
            var womenStatus = new List<PartnerTypeAge>();
            var menStatus = new List<PartnerTypeAge>();
            var femaleAge = Age.FemaleWeights;
            var maleAge = Age.MaleWeights;

            //Act
            for (int i = 18; i < 100; i++) 
            {
                womenList.Add(i, new List<PartnerTypeAge>());
                menList.Add(i, new List<PartnerTypeAge>());

                for (int j = 0; j < femaleAge[i]; j++) 
                {
                    weights = frame.GetColumn<double>(Convert.ToString(i)).Values.Select(c => Convert.ToDouble(c)).Skip(3).Take(3).ToList();
                    MaritalStatusAge.Weights = weights;
                    womenList[i].Add(env.RandChoice(MaritalStatusAge.Source, MaritalStatusAge.Weights));
                }

                for (int j = 0; j < maleAge[i]; j++) 
                {
                    weights = frame.GetColumn<double>(Convert.ToString(i)).Values.Select(c => Convert.ToDouble(c)).Take(3).ToList();
                    MaritalStatusAge.Weights = weights;
                    menList[i].Add(env.RandChoice(MaritalStatusAge.Source, MaritalStatusAge.Weights));
                }

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
            var export = Frame.FromRecords(collection);
            export.SaveCsv("../../../../../doc/data/maritalAgeProbability.csv");
        }
    }
}
