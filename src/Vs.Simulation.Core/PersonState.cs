﻿using System;
using System.Collections.Generic;
using Vs.Simulation.Core.Probabilities;

namespace Vs.Simulation.Core
{
    /// <summary>
    /// Persons state including state machine. The state machine is used for sanity checks in the
    /// lifecycle process events of the person during its lifespan.
    /// !Note, the state machine could be upgraded to be the overal leading controller of the DES process flow.
    /// At this time we focus on DES processes itself setting the state machine instead.
    /// </summary>
    public class PersonState : BaseStateMachine<LifeEvents, LifeEventsTriggers>
    {
        public PersonState(LifeEvents initialState) : base(initialState)
        {
            Machine.Configure(LifeEvents.Born)
                .Permit(LifeEventsTriggers.Die, LifeEvents.Deceased)
                .Permit(LifeEventsTriggers.Adulthood, LifeEvents.Adult);
            Machine.Configure(LifeEvents.Adult)
                .Permit(LifeEventsTriggers.Die, LifeEvents.Deceased)
                .Permit(LifeEventsTriggers.Mary, LifeEvents.Married);
            Machine.Configure(LifeEvents.Married)
                .Permit(LifeEventsTriggers.Die, LifeEvents.Deceased)
                .Permit(LifeEventsTriggers.Divorce, LifeEvents.Divorced);
            Machine.Configure(LifeEvents.Divorced)
                .Permit(LifeEventsTriggers.Die, LifeEvents.Deceased)
                .Permit(LifeEventsTriggers.Mary, LifeEvents.Married);
            Machine.Activate();
        }

        public SexType Sex { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfDeath { get; set; }
        public TimeSpan Lifespan { get; set; }
        public List<PersonObject> Partners { get; set; }
        public List<PersonObject> Sibblings { get; set; }
        public List<PersonObject> Parents { get; set; }
    }

    public enum LifeEvents
    {
        Born,
        Adult,
        Deceased,
        Married,
        Divorced
    }

    public enum LifeEventsTriggers
    {
        Birth,
        Adulthood,
        Die,
        Mary,
        Divorce
    }

    public class StateEvent<T>
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public T State { get; set; }

        public StateEvent(int id, DateTime dataTime, T state)
        {
            Id = id;
            DateTime = dataTime;
            State = state;
        }
    }
}
