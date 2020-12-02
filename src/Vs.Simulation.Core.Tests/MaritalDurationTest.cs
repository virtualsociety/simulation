using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Deedle;
using Vs.Simulation.Core.Probabilities;
using System.Linq;

namespace Vs.Simulation.Core.Tests
{
    public class MaritalDurationProbability 
    {
        public double Duration { get; set; }
        public int MarriedDurationCouples { get; set; }
    }


     public class MaritalProbability<T>
    {

        private List<T> _weights;
        private List<T> _source;

         MaritalProbability(List<T> weights, List<T> source)
        {
            _weights = weights;
            _source = source;
        }



    }


    public class MaritalDurationCollection : List<MaritalDurationProbability> 
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
            //The married couples are randomly assigned into a marital duration list, using the duration weights given in the "MaritalDuration" class
            for (int i = 0; i < 3217582; i++) 
            {
                coupleList.Add(env.RandChoice(MaritalDuration.DurationSource, MaritalDuration.DurationWeights));
            }
            //The couples are sorted into a collection based on their i variable. 
            // i is in this case the years someone is married, the duration.

            for (double i = 0; i < 72; i++) {
                collection.Add(new MaritalDurationProbability()
                {
                    Duration = i,             
                    MarriedDurationCouples = coupleList.Where(d => d == i).Count()
                });
            }

            //Collection is exported into a CSV file saved in the location below.
            var export = Frame.FromRecords(collection);
            export.SaveCsv("../../../../../doc/data/maritalDurationList.csv");

            
            //Assert
            //We took 3 random durations from the list and we asserted them to see if they are 
            //really the same number each time.
            //We tested it to 5 years, 31 years and, 54 years.
            int actualDuration5 = coupleList.Where(d => d == 5).Count();
            int expectedDuration5 = 59944;
            int actualDuration54 = coupleList.Where(d => d == 54).Count();
            int expectedDuration54 = 34631;
            int actualDuration31 = coupleList.Where(d => d == 31).Count();
            int expectedDuration31 = 53246;

            Assert.Equal(expectedDuration5, actualDuration5);
            Assert.Equal(expectedDuration31, actualDuration31);
            Assert.Equal(expectedDuration54, actualDuration54);


        }
    }
}
