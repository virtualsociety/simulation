using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Deedle;
using Vs.Simulation.Core.Probabilities;
using System.Linq;

namespace Vs.Simulation.Core.Tests
{
    public class MaritalDurationPropability 
    {
        public double Duration { get; set; }
        public int MarriedDurationCouples { get; set; }
    }
    public class MaritalDurationCollection : List<MaritalDurationPropability> 
    {
    }
    public class MaritalDurationTest
    {
        [Fact]
        public static void MaritalDuration1() 
        {
            //Arrange
            var env = new SimSharp.ThreadSafeSimulation(42);
            var collection = new MaritalDurationCollection();
            var coupleList = new List<double>();
            //Act
            for (int i = 0; i < 10000; i++) 
            {
                coupleList.Add(env.RandChoice(MaritalDuration.DurationSource, MaritalDuration.DurationWeights));
            }
            for (double i = 1; i < 22; i++) {
                collection.Add(new MaritalDurationPropability()
                {
                    Duration = i,
                    MarriedDurationCouples = coupleList.Where(d => d == i).Count()
                });
            }
            var export = Frame.FromRecords(collection);
            export.SaveCsv("../../../../../doc/data/maritalDurationList.csv");
            
        }
    }
}
