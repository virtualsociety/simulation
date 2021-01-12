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

            public Person(SimSharp.Simulation environment, List<Person> parents = null) : base(environment)
            {
                Persons.Add(this);
                if (parents != null)
                {
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
                }
                _data.children = new List<Person>();
                _data.Flags = new BitArray(2);

                _data.Id = Global._counter++;

                _data.Dob = Environment.Now;
                _data.Year = _data.Dob.Year;
                Events.Write(new Triple() { Subject = _data.Id, Predicate = Constants.triple_predicate_child_of, Time=Environment.Now, Object = 0 });
                // Determine Gender
                _data.Flags[Constants.idx_gender] = Environment.RandChoice(GenderProbability.Source, GenderProbability.Weights);
                Statistics.People[Convert.ToByte(_data.Flags[Constants.idx_gender])]++;
                //Persons[Gender].Push(this);
                // Start the "LifeCycle"
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
