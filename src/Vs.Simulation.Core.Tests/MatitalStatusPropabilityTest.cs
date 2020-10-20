using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Vs.Simulation.Core;
using Vs.Simulation.Core.Probabilities;
using Deedle;
using System.Linq;

namespace Vs.Simulation.Core.Tests
{
    

    public class MatitalStatusPropabilityTest
    {
  
        [Fact]
        public static void MartialStatus() 
        {
            var env = new SimSharp.ThreadSafeSimulation(42);

            var m = env.RandChoice(MaritalStatus.Source, MaritalStatus.Weights);
        }


        [Fact]
        public static void ReadFile() 
        {
            //Arrange
             Frame<int, string> frame;
             List<double> weights;
             List<double> source;

            // Act
            frame =  Frame.ReadCsv("../../../../../doc/data/marital_status.csv");
            weights = frame.GetColumn<double>("2019").Values.Select(c => Convert.ToDouble(c)).Take(3).ToList();
            MaritalStatus.Weights = weights;
            // source = frame.GetColumn<double>("2019").Keys.Select(c => Convert.ToString(c)).ToList();

            var env = new SimSharp.ThreadSafeSimulation(42);
            var people = new List<PartnerType>();
            for (int i = 0; i < 250;i++) {
                people.Add(env.RandChoice(MaritalStatus.Source, MaritalStatus.Weights));
            }

            // Assert
            var singles = people.Where(p => p == PartnerType.Single).Count();
            Assert.Equal(114, singles);

        }
    }
}