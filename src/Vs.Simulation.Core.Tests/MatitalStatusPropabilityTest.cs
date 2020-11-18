using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Vs.Simulation.Core;
using Vs.Simulation.Core.Probabilities;
using Deedle;
using System.Linq;
using System.IO;
using System.Globalization;

namespace Vs.Simulation.Core.Tests
{
    public class MaritalStatusProbability
    {
        public int Year { get; set; }
        public int Singles { get; set; }
        public int Married { get; set; }
        public int Partnership { get; set; }

    }

    public class MaritalStatusProbabilityCollection:List<MaritalStatusProbability> 
    {

    }

    public class MatitalStatusProbabilityTest
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
            // Arrange
            Frame<int, string> frame;
            List<double> weights;
            var collection = new MaritalStatusProbabilityCollection();
            frame =  Frame.ReadCsv("../../../../../doc/data/marital_status.csv");

            var env = new SimSharp.ThreadSafeSimulation(42);

            // Act
            for (int j = 1950; j < 2020; j++)
            { 
                var people = new List<PartnerType>();

                weights = frame.GetColumn<double>(Convert.ToString(j)).Values.Select(c => Convert.ToDouble(c)).Take(3).ToList();
                MaritalStatus.Weights = weights;
                for (int i = 0; i < 100000; i++)
                {
                    people.Add(env.RandChoice(MaritalStatus.Source, MaritalStatus.Weights));
                }
                collection.Add(new MaritalStatusProbability()
                {
                    Singles = people.Where(p => p == PartnerType.Single).Count(),
                    Married = people.Where(p => p == PartnerType.Married).Count(),
                    Partnership = people.Where(p => p == PartnerType.Partnership).Count(),
                    Year = j
                });
            }
            var export = Frame.FromRecords(collection);
            export.SaveCsv("../../../../../doc/data/maritalStatusWeights.csv");
        }
    } 
}