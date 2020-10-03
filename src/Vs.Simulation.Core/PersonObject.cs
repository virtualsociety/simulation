using Deedle;
using SimSharp;
using System;
using System.Collections.Generic;
using Vs.Simulation.Core.Probabilities;
using System.Linq;

namespace Vs.Simulation.Core
{
    /// <summary>
    /// A person visits serveral life events, as set forward in the Person State Machine.
    /// Instantiating a person object marks its birth and death.
    /// </summary>
    public class PersonObject : ActiveObject<SimSharp.Simulation>
    {
        /// <summary>
        /// Person LifeEvent Stream
        /// <para>+----+---------------------+-------+</para>
        /// <para>| Id |      DateTime       | State |</para>
        /// <para>+----+---------------------+-------+</para>
        /// <para>|  0 | 02/28/2014 18:41:28 | Born  |</para>
        /// <para>|  1 | 03/28/2014 13:21:04 | Born  |</para>
        /// <para>+----+---------------------+-------+</para>
        /// </summary>
        public static List<StateEvent<LifeEvents>> _events = new List<StateEvent<LifeEvents>>();

        /// <summary>
        /// Person unique identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The person's current state
        /// </summary>
        public PersonState State { get; private set; }
        public Process LifeCycleProcess { get; private set; }
        private Process AdultProcess { get; set; }

        public PersonObject(int id, SimSharp.Simulation environment) : base(environment)
        {
            Id = id;
            State = new PersonState(LifeEvents.Born)
            {
                Sex = environment.RandChoice(Sex.Source, Sex.Weights),
                DateOfBirth = environment.Now,
            };
            if (State.Sex == SexType.Male)
            {
                State.Lifespan = TimeSpan.FromDays(Environment.RandChoice(Age.MaleSource, Age.MaleWeights) * 365);
            }
            else
            {
                State.Lifespan = TimeSpan.FromDays(Environment.RandChoice(Age.FemaleSource, Age.FemaleWeights) * 365);
            }
            LifeCycleProcess = Environment.Process(LifeCycle());
        }

        /// <summary>
        /// The lifecycle of the person runs until age of death has been reached.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Event> LifeCycle()
        {
            _events.Add(new StateEvent<LifeEvents>(Id, Environment.Now, State.Machine.State));
            AdultProcess = Environment.Process(Adult());
            yield return Environment.Timeout(State.Lifespan);
            if (AdultProcess.IsAlive)
            {
                // death angel interuption of the adult process and all its child processes.
                AdultProcess.Interrupt();
            }
            State.Machine.Fire(LifeEventsTriggers.Die);
            _events.Add(new StateEvent<LifeEvents>(Id, Environment.Now, State.Machine.State));
            State.DateOfDeath = Environment.Now;

        }

        private IEnumerable<Event> Adult()
        {
            yield return Environment.Timeout(new TimeSpan(18 * 365));
        }
    }
}
