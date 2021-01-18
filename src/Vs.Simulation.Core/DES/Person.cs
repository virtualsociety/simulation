using SimSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using Vs.Simulation.Core.Probabilities;
using Vs.Simulation.Shared;
using Vs.Simulation.Shared.Model;

namespace Vs.Simulation.Core
{

    public partial class Population
    {
        /*
         * A Person is born, reaches adulthood, gets married 
         * and has children
         * 
         */
        public partial class Person : ActiveObject<SimSharp.Simulation>
        {
            public struct Data
            {
                public BitArray Flags;
                public long Id;
                public DateTime Dob;
                public TimeSpan End;
                public List<Person> children;
                public List<Person> parents;
                public List<DateRange<Person>> partners;
                public DateTime Dod;
                public int Year;      // for performance
                public int YearDod;   // for performance
            }

            public Data _data = new Data();
            public Process Process;

            /// <summary>
            /// For warm up procedure create a person of a certain start age, instead of being born at simulation time.
            /// This person will not have parents. This is where the history ends.
            /// </summary>
            /// <param name="environment"></param>
            /// <param name="age"></param>
            public Person(SimSharp.Simulation environment, int age) : base(environment)
            {
                CreatePersonInitialData(environment.Now.AddYears(-age));
                Persons.Add(this);
                // Person is born with no parents (before the simulation time)
                Events.Write(new Triple() { Subject = _data.Id, Predicate = Constants.triple_predicate_child_of, Time = Environment.Now.AddYears(-age), Object = 0 });
                Process = environment.Process(LifeCycle());
            }

            private void CreatePersonInitialData(DateTime dateOfBirth)
            {
                _data.children = new List<Person>();
                _data.Flags = new BitArray(2);
                _data.Id = Global._counter++;
                _data.Dob = dateOfBirth;
                _data.Year = _data.Dob.Year;
                // Determine Gender
                _data.Flags[Constants.idx_gender] = Environment.RandChoice(GenderProbability.Source, GenderProbability.Weights);
                Statistics.People[Convert.ToByte(_data.Flags[Constants.idx_gender])]++;
            }

            public Person(SimSharp.Simulation environment, List<Person> parents) : base(environment)
            {
                Persons.Add(this);
                _data.parents = parents;
                Statistics.Children++;
                _data.parents[0]._data.children.Add(this);
                _data.parents[1]._data.children.Add(this);

                Events.Write(new Triple()
                {
                    Subject = _data.Id,
                    Predicate = Constants.triple_predicate_child_of,
                    InvPredicate = Constants.triple_predicate_parent_of,
                    Time = Environment.Now,
                    Object = _data.parents[0]._data.Id
                });
                Events.Write(new Triple()
                {
                    Subject = _data.Id,
                    Predicate = Constants.triple_predicate_child_of,
                    InvPredicate = Constants.triple_predicate_parent_of,
                    Time = Environment.Now,
                    Object = _data.parents[1]._data.Id
                });

                CreatePersonInitialData(Environment.Now);
                Process = environment.Process(LifeCycle());
            }

            public double SimulationAge
            {
                get
                {
                    return (Environment.Now - _data.Dob).TotalDays / 365;
                }
            }
        }
    }
}
