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
        /// <summary>
        /// Name of the simulation
        /// </summary>
        public string Name { get; private set; }
        public TimeSpan SimTime { get; }

        /// <summary>
        /// The main process thread of the simulation, which can be interrupted.
        /// </summary>
        public Process Process { get; private set; }
        /// <summary>
        /// Population containing all person simulation subjects and their state.
        /// </summary>
        public static List<PersonObject> Persons { get; private set; } = new List<PersonObject>();

        private static long _counter = 0;

        public static long Counter() 
        {

            return Interlocked.Increment(ref _counter);
        }

        public Population(SimSharp.Simulation env, string name, TimeSpan simTime)
          : base(env)
        {
            Db = new PopulationDb();
            Name = name;
            SimTime = simTime;
            // Start the birth process
            //Process = Environment.Process(BirthProcess());
            Process = Environment.Process(WarmupProcess());
            //Process = Environment.Process(BirthProcess());
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
                // start a new persons life cycle and add the person to the list for later reporting.
                Persons.Add(new PersonObject(i, Environment, SimTime));
                i++;
            }
        }

        private IEnumerable<Event> WarmupProcess() 
        {
            int i = 0;
            //while (true)
           // {
                while (i < 1000)
                {
                    yield return Environment.Timeout(TimeSpan.FromSeconds(1));
                    // start a new persons life cycle and add the person to the list for later reporting.
                    Persons.Add(new PersonObject(i, Environment, SimTime));
                    i++;
                }
           // }
        }

        //int i = 166558;
        //public IEnumerable<Event> BirthSubProcess() 
        //{
        //
        //    Persons.Add(new PersonObject(i, Environment, SimTime));
        //    i++;
        //    yield return Environment.Timeout(TimeSpan.FromDays(2 * 365));
        //}
    }
}
