using Deedle;
using SimSharp;
using System;
using Vs.Simulation.Core;
using System.Linq;
using Vs.Simulation.Core.Probabilities;

namespace Vs.Simulation.Terminal
{
    public class Simulation
    {
        /// <summary>
        /// Random seed for random distribution
        /// </summary>
        private const int RandomSeed = 42;
        /// <summary>
        /// Simulation time in Days
        /// </summary>
        public static readonly TimeSpan SimTime = TimeSpan.FromDays(69 * 365);

        /// <summary>
        /// Active object in the simulation, representing the population.
        /// </summary>

        public void Simulate(int rseed = RandomSeed)
        {
            // Setup and start the simulation
            // Create an environment and start the setup process
            var start = new DateTime(1950, 2, 1);
            var env = new SimSharp.ThreadSafeSimulation(start, rseed);
            env.Log("== Population ==");
            var population = new Population(env, "Virtual Society", SimTime);
            var startPerf = DateTime.UtcNow.AddYears(-1);
            env.Run(SimTime);
            var perf = DateTime.UtcNow.AddYears(-1) - startPerf;
            // Analysis / results
            env.Log("Population results after {0} days.", (env.Now - start).TotalDays);
            env.Log("Population {0} has {1} babies born.", population.Name, Population.Db.People.Count);
            env.Log(string.Empty);
            env.Log("Processed {0} events in {1} seconds ({2} events/s).", env.ProcessedEvents, perf.TotalSeconds, (env.ProcessedEvents / perf.TotalSeconds));
            env.Log("Preparing statistics");
            // Create data frames for reporting to csv for tooling such as excel.
            env.Log("Selecting Males");
            var males = from p in Population.Db.People
                        where p.Gender == GenderType.Male
                        select new
                        {
                            p.Id,
                            p.DateOfBirth,
                            p.DateOfDeath,
                            p.Lifespan.TotalDays,
                            p.Age
                        };
            env.Log("Males {0}", males.Count());
            env.Log("Selecting Females");
            var females = from p in Population.Db.People
                        where p.Gender == GenderType.Female
                        select new
                        {
                            p.Id,
                            p.DateOfBirth,
                            p.DateOfDeath,
                            p.Lifespan.TotalDays,
                            p.Age
                        };
            env.Log("Females {0}", females.Count());
            env.Log("Ratio Males/Females {0}",(double) males.Count() / females.Count());
            // +----+---------------------+---------------------+-----------+-----+
            // | Id |     DateOfBirth     |     DateOfDeath     | TotalDays | Age |
            // +----+---------------------+---------------------+-----------+-----+
            // |  1 | 02/01/1916 13:21:58 | 01/15/1984 13:21:58 |     24820 |  68 |
            // |  2 | 02/01/1916 20:01:58 | 01/23/1951 20:01:58 |     12775 |  35 |
            // +----+---------------------+---------------------+-----------+-----+ https://ozh.github.io/ascii-tables/
            var frames = Frame.FromRecords(males);
            var median = Stats.median(frames.GetColumn<double>("Age"));
            env.Log("Saving Males. Median age {0}", median);
            frames.SaveCsv("males.csv");
            frames = Frame.FromRecords(females);
            median = Stats.median(frames.GetColumn<double>("Age"));
            env.Log("Saving Females. Median age {0}", median);
            frames.SaveCsv("females.csv");
            env.Log("Saving Event Stream");
            // +----+---------------------+-------+
            // | Id |      DateTime       | State |
            // +----+---------------------+-------+
            // |  0 | 02/28/2014 18:41:28 | Born  |
            // |  1 | 03/28/2014 13:21:04 | Born  |
            // +----+---------------------+-------+ 
            Frame.FromRecords(PersonObject._events).SaveCsv(@"life-events.csv");
            env.Log("Finished..");
        }
    }
}