using Deedle;
using SimSharp;
using System;
using System.Collections.Generic;
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
        private static readonly TimeSpan SimTime = TimeSpan.FromDays(104 * 365);

        /// <summary>
        /// Active object in the simulation, representing the population.
        /// </summary>
        private class Population : ActiveObject<SimSharp.Simulation>
        {
            /// <summary>
            /// Avg. processing time in minutes
            /// </summary>
            private static readonly TimeSpan BirthMean = TimeSpan.FromMinutes(400);
            /// <summary>
            /// Sigma of processing time
            /// </summary>
            private static readonly TimeSpan BirthSigma = TimeSpan.FromMinutes(1);
            /// <summary>
            /// Name of the simulation
            /// </summary>
            public string Name { get; private set; }
            /// <summary>
            /// The main process thread of the simulation, which can be interupted.
            /// </summary>
            public Process Process { get; private set; }
            /// <summary>
            /// Population containing all person simulation subjects and their state.
            /// </summary>
            public List<PersonObject> persons { get; private set; } = new List<PersonObject>();
            
            public Population(SimSharp.Simulation env, string name)
              : base(env)
            {
                Name = name;
                // Start the birth process
                Process = Environment.Process(BirthProcess());
            }

            /// <summary>
            ///  Produce babies as long as the simulation runs.
            /// </summary>
            /// <returns></returns>
            private IEnumerable<Event> BirthProcess()
            {
                int i = 0;
                while (true)
                {
                    // Start creating a new baby.
                    var doneIn = Environment.RandNormalPositive(BirthMean, BirthSigma);
                    yield return Environment.Timeout(doneIn);
                    // start a new persons lifecycle and add the person to the list for later reporting.
                    persons.Add(new PersonObject(i, Environment/*, Environment.RandNormalPositive(TimeSpan.FromDays(57 * 365), TimeSpan.FromDays(57 * 365))*/));
                    i++;
                }
            }
        }

        public void Simulate(int rseed = RandomSeed)
        {
            // Setup and start the simulation
            // Create an environment and start the setup process
            var start = new DateTime(2020-104, 2, 1);
            var env = new SimSharp.Simulation(start, rseed);
            env.Log("== Population ==");
            var population = new Population(env, "Virtual Society");
            var startPerf = DateTime.UtcNow;
            env.Run(SimTime);
            var perf = DateTime.UtcNow - startPerf;
            // Analyis / results
            env.Log("Population results after {0} days.", (env.Now - start).TotalDays);
            env.Log("Population {0} has {1} babies born.", population.Name, population.persons.Count);
            env.Log(string.Empty);
            env.Log("Processed {0} events in {1} seconds ({2} events/s).", env.ProcessedEvents, perf.TotalSeconds, (env.ProcessedEvents / perf.TotalSeconds));
            env.Log("Preparing statistics");
            // Create data frames for reporting to csv for tooling such as excel.
            env.Log("Selecting Males");
            var males = from p in population.persons
                        where p.State.Sex == SexType.Male
                        select new
                        {
                            p.Id,
                            p.State.DateOfBirth,
                            p.State.DateOfDeath,
                            p.State.Lifespan.TotalDays,
                            p.State.Age
                        };
            env.Log("Males {0}", males.Count());
            env.Log("Selecting Females");
            var females = from p in population.persons
                        where p.State.Sex == SexType.Female
                        select new
                        {
                            p.Id,
                            p.State.DateOfBirth,
                            p.State.DateOfDeath,
                            p.State.Lifespan.TotalDays,
                            p.State.Age
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
            frames.SaveCsv(@"females.csv");
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