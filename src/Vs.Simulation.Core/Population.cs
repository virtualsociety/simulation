using SimSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Vs.Simulation.Core.Database;

namespace Vs.Simulation.Core
{
    public class Population : ActiveObject<SimSharp.Simulation>
    {
        /// <summary>
        /// Population Database containing all person simulation subjects and their state.
        /// </summary>
        public static PopulationDb Db { get; private set; }

        /// <summary>
        /// Avg. processing time in minutes
        /// </summary>
        private static readonly TimeSpan BirthMean = TimeSpan.FromMinutes(40000);
        /// <summary>
        /// Sigma of processing time
        /// </summary>
        private static readonly TimeSpan BirthSigma = TimeSpan.FromMinutes(1);

        public static SimSharp.Simulation Env { get; private set; }

        /// <summary>
        /// Name of the simulation
        /// </summary>
        public string Name { get; private set; }
        public TimeSpan SimTime { get; }
        public int Size { get; }

        /// <summary>
        /// The main process thread of the simulation, which can be interrupted.
        /// </summary>
        public Process Process { get; private set; }
        /// <summary>
        /// Population containing all person simulation subjects and their state.
        /// </summary>
        public static List<PersonObject> Persons { get; private set; } = new List<PersonObject>();

        private static long _counter = 0;

        /// <summary>
        /// Thread safe counter for person id's.
        /// </summary>
        /// <returns></returns>
        public static long Counter() 
        {

            return Interlocked.Increment(ref _counter);
        }

        /// <summary>
        /// Creates a new population initiating a warm up process, which then transitions into pseudo real time simulation.
        /// </summary>
        /// <param name="env"></param>
        /// <param name="name"></param>
        /// <param name="simTime"></param>
        public Population(SimSharp.Simulation env, string name, TimeSpan simTime, int size)
          : base(env)
        {
            Db = new PopulationDb();
            Env = env;
            Name = name;
            SimTime = simTime;
            Size = size;
            Process = Environment.Process(WarmupProcess());
        }

        private IEnumerable<Event> WarmupProcess()
        {
            while (_counter < Size)
            {
                yield return Environment.Timeout(TimeSpan.FromSeconds(1));
                // start a new persons life cycle and add the person to the list for later reporting.
                Persons.Add(new PersonObject(Counter(), Environment, SimTime));
            }
        }
    }
}
