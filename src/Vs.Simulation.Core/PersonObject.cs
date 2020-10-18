using SimSharp;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Person Entity for table storage
        /// </summary>
        public PersonEntity Person { get; set; } = new PersonEntity();

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

        public TimeSpan SimTime { get; }

        /// <summary>
        /// The person's current state
        /// </summary>
        public PersonState State { get; private set; }
        public Process LifeCycleProcess { get; private set; }
        public Process DeathProcess { get; set; }

        private Process AdultProcess { get; set; }

        public PersonObject(int id, SimSharp.Simulation environment, TimeSpan simTime) : base(environment)
        {
            Person.Id = id;
            SimTime = simTime;
            State = new PersonState(LifeEvents.Born);
            Person.Sex = environment.RandChoice(Sex.Source, Sex.Weights);
            Person.DateOfBirth = environment.Now;
            _events.Add(new StateEvent<LifeEvents>(Person.Id, Environment.Now, State.Machine.State));
            if (!finnishHandle)
            {
                finnishHandle = true;
                Environment.RunFinished += Environment_RunFinished;
            }
            LifeCycleProcess = Environment.Process(LifeCycle());
            Population.Db.People.Insert(this.Person);

        }

        static bool finnishHandle;
        static bool _stopped;

        static void Environment_RunFinished(object sender, EventArgs e)
        {
            _stopped = true;
        }

        private IEnumerable<Event> Death()
        {
            if (Person.Sex == SexType.Male)
            {
                Person.Lifespan = TimeSpan.FromDays(1+ Environment.RandChoice(Age.MaleSource, Age.MaleWeights) * 365);
            }
            else
            {
                Person.Lifespan = TimeSpan.FromDays(1+ Environment.RandChoice(Age.FemaleSource, Age.FemaleWeights) * 365);
            }
            Population.Db.People.Update(this.Person);

            yield return Environment.Timeout(Person.Lifespan);
            LifeCycleProcess.Interrupt();
            State.Machine.Fire(LifeEventsTriggers.Die);
            Person.LifeEvent = LifeEvents.Deceased;
            _events.Add(new StateEvent<LifeEvents>(Person.Id, Environment.Now, State.Machine.State));
            Person.DateOfDeath = Environment.Now;
            Population.Db.People.Update(this.Person);

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

        private IEnumerable<Event> Marriage()
        {
            var m = Environment.RandChoice(MaritalStatus.Source, MaritalStatus.Weights);
            switch (m)
            {
                //case PartnerType.Single:
                //    yield return Environment.Timeout(TimeSpan.FromSeconds(0));
                //    break;
                case PartnerType.Married:
                case PartnerType.Partnership:
                    // Schedule marriage within lifespan
                    //yield return Environment.Timeout(TimeSpan.FromDays(Environment.RandNormal(State.Lifespan.Days,0)));
                    yield return Environment.Timeout(TimeSpan.FromDays(100));
                    // Transition state into married
                    if (Person.LifeEvent == LifeEvents.Adult)
                    {
                        State.Machine.Fire(LifeEventsTriggers.Mary);
                        Person.LifeEvent = LifeEvents.Married;
                        var partner = Population.Db.People.First(p => p.LifeEvent == LifeEvents.Adult);
                        partner.LifeEvent = LifeEvents.Married;
                        //partner.reference.State.Partners.Add(this);
                        //State.Partners.Add(partner.reference);
                        _events.Add(new StateEvent<LifeEvents>(Person.Id, Environment.Now, Person.LifeEvent));
                        _events.Add(new StateEvent<LifeEvents>(partner.Id, Environment.Now, partner.LifeEvent));
                        Population.Db.People.Update(partner);
                        Population.Db.People.Update(this.Person);
                    }
                    break;
            }
        }

        private IEnumerable<Event> Adult()
        {
            // #1 schedule adulthood, the subject reaches 18 years (legal age)
            yield return Environment.Timeout(TimeSpan.FromDays((Person.DateOfBirth.AddYears(18).Date - Person.DateOfBirth).TotalDays));
            // the subject should not be deceased
            if (Person.LifeEvent != LifeEvents.Deceased)
            {
                Environment.Process(Marriage());
                State.Machine.Fire(LifeEventsTriggers.Adulthood);
                Person.LifeEvent = LifeEvents.Adult;
                _events.Add(new StateEvent<LifeEvents>(Person.Id, Environment.Now, State.Machine.State));
                Population.Db.People.Update(this.Person);
                // Schedule for marrital status
            }
        }
    }
}
