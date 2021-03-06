﻿using System;
using System.Collections.Generic;
using Xunit;
using Vs.Simulation.Core.Probabilities;
using Deedle;
using System.Linq;

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
        {  }


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
            //For the years 1950 to 2019 we are going to put a test amount of people through the weights and see what happens
            for (int j = 1950; j < 2020; j++)
            { 
                //A list partnertype made, where we will put everyone in.
                var people = new List<PartnerType>();

                //Weights are decided from "marital_status.csv", each year takes their own amount of weights. 
                weights = frame.GetColumn<double>(Convert.ToString(j)).Values.Select(c => Convert.ToDouble(c)).Take(3).ToList();
                MaritalStatus.Weights = weights;

                //For 100000 citizens of each year, they each get randomly sorted in a source, based on the
                //weights given above.
                for (int i = 0; i < 100000; i++)
                {
                    people.Add(env.RandChoice(MaritalStatus.Source, MaritalStatus.Weights));
                }

                //Here we are added all of them into a collection based on the MaritalStatusPorbability class which can be viewed at the top.
                collection.Add(new MaritalStatusProbability()
                {
                    Singles = people.Where(p => p == PartnerType.Single).Count(),
                    Married = people.Where(p => p == PartnerType.Married).Count(),
                    Partnership = people.Where(p => p == PartnerType.Partnership).Count(),
                    Year = j
                });
            }
            //This gets exported into a CSV file which can be located as stated below.
            var export = Frame.FromRecords(collection);
            export.SaveCsv($"{Global.GetDataFolder()}maritalStatusWeights.csv");
        }
    } 
}