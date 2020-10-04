using SimSharp;
using System;
using System.Collections.Generic;
using Vs.Simulation.Core.Probabilities;

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
        public TimeSpan SimTime { get; }

        /// <summary>
        /// The person's current state
        /// </summary>
        public PersonState State { get; private set; }
        public Process LifeCycleProcess { get; private set; }
        /// <summary>
        /// The Life Events Process
        /// </summary>
        public Process LifeEventsProcess { get; private set; }
        public Process DeathProcess { get; set; }

        private Process AdultProcess { get; set; }

        public PersonObject(int id, SimSharp.Simulation environment, TimeSpan simTime) : base(environment)
        {
            Id = id;
            SimTime = simTime;
            State = new PersonState(LifeEvents.Born)
            {
                Sex = environment.RandChoice(Sex.Source, Sex.Weights),
                DateOfBirth = environment.Now,
            };
            _events.Add(new StateEvent<LifeEvents>(Id, Environment.Now, State.Machine.State));
            if (!finnishHandle)
            {
                finnishHandle = true;
                Environment.RunFinished += Environment_RunFinished;
            }
            LifeCycleProcess = Environment.Process(LifeCycle());
        }

        static bool finnishHandle;
        static bool _stopped;

        static void Environment_RunFinished(object sender, EventArgs e)
        {
            _stopped = true;
        }

        /// <summary>
        /// The main scheduler for Life events schedules.
        /// This process runs within the scope of the lifecycle of the person
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Event> LifeEventsSchedules()
        {
            // pre-determined life events
            // #1 schedule adulthood, the subject reaches 18 years (legal age)
            yield return Environment.Timeout(TimeSpan.FromDays((State.DateOfBirth.AddYears(18).Date - State.DateOfBirth).TotalDays));
            LifeCycleProcess.Interrupt(LifeEvents.Adult);
            while (true)
            {
                yield return Environment.Timeout(TimeSpan.FromDays(1));
            }
        }

        private IEnumerable<Event> Death()
        {
            if (State.Sex == SexType.Male)
            {
                State.Lifespan = TimeSpan.FromDays(1+ Environment.RandChoice(Age.MaleSource, Age.MaleWeights) * 365);
            }
            else
            {
                State.Lifespan = TimeSpan.FromDays(1+ Environment.RandChoice(Age.FemaleSource, Age.FemaleWeights) * 365);
            }
            yield return Environment.Timeout(State.Lifespan);
            LifeCycleProcess.Interrupt();
            State.Machine.Fire(LifeEventsTriggers.Die);
            _events.Add(new StateEvent<LifeEvents>(Id, Environment.Now, State.Machine.State));
            State.DateOfDeath = Environment.Now;
        }

        /// <summary>
        /// The lifecycle of the person runs until age of death has been reached.
        /// It gets interupted now and then by the Life Events Process.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Event> LifeCycle()
        {
            AdultProcess = Environment.Process(Adult());
            DeathProcess = Environment.Process(Death());
            // Do not run any lifeCycle longer than the simulation time.
            yield return Environment.Timeout(SimTime);
            bool stopped = false;
            while (!stopped && !_stopped)
            {
                // lifecycle process hearbeat

                // Angel of Death interrupts thhe lifecycle
                if (LifeCycleProcess.HandleFault())
                {
                    stopped = true;
                }
            }
        }

        private IEnumerable<Event> Adult()
        {
            // #1 schedule adulthood, the subject reaches 18 years (legal age)
            yield return Environment.Timeout(TimeSpan.FromDays((State.DateOfBirth.AddYears(18).Date - State.DateOfBirth).TotalDays));
            // the subject should not be deceased
            if (!State.Machine.IsInState(LifeEvents.Deceased))
            {
                State.Machine.Fire(LifeEventsTriggers.Adulthood);
                _events.Add(new StateEvent<LifeEvents>(Id, Environment.Now, State.Machine.State));
            }
        }
    }
}
