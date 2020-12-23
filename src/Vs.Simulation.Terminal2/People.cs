using Microsoft.VisualBasic;
using SimSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Vs.Simulation.Core;
using Vs.Simulation.Terminal2.Probabilities;

namespace Vs.Simulation.Terminal2
{
    public class DateRange<T>
    {
        public T Object {get;set;}
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }

        public DateRange(T @object, DateTime start)
        {
            Object = @object;
            Start = start;
        }
    }

    public static class Global
    {
        public static int _counter;
        public static long[] _totalAge = new long[2];
    }

    /// <summary>
    /// People Example
    /// </summary>
    public class People : ActiveObject<SimSharp.Simulation>
    {
        public static EventStore Events = new EventStore();
        /// <summary>
        /// Unmarried Persons by Gender
        /// </summary>
        public static Stack<Person>[] Unmarried = new Stack<Person>[2];
        public static List<Person> Persons = new List<Person>();
        public Process Process { get; private set; }

        public People(SimSharp.Simulation environment) : base(environment)
        {
            Events.Write(new Triple() { Subject =0, Predicate = Constants.triple_predicate_created, Time = Environment.Now, Object = 0 });

            //Persons.Add(GenderType.Male, new Stack<Person>());
            // Persons.Add(GenderType.Female, new Stack<Person>());
            Unmarried[Constants.idx_gender_male] = new Stack<Person>();
            Unmarried[Constants.idx_gender_female] = new Stack<Person>();

            Process = environment.Process(Warmup());
        }

        public IEnumerable<Event> Warmup()
        {
            while (true)
            {
                // average 1 per person hour creation.
                yield return Environment.Timeout(TimeSpan.FromMinutes(Environment.RandNormal(20, 0)));
                new Person(Environment);
            }
        }

        /*
         * A Person is born, reaches adulthood, gets married 
         * and has children
         * 
         */
        public class Person : ActiveObject<SimSharp.Simulation>
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
                _data.Flags[Constants.idx_gender] = Environment.RandChoice(Gender.Source, Gender.Weights);
                Statistics.People[Convert.ToByte(_data.Flags[Constants.idx_gender])]++;
                //Persons[Gender].Push(this);
                // Start the "LifeCycle"
                Process = environment.Process(LifeCycle());
            }

            private IEnumerable<Event> Death()
            {
                yield return Environment.Timeout(_data.End);
                People.Events.Write(new Triple() { 
                    Subject = _data.Id, 
                    Predicate = Constants.triple_predicate_died_of, 
                    Object = _data.Id, Time = Environment.Now 
                });
                Statistics.Deaths++;

                //If the person was married, make the partner a widow and avaiable for marriage again.
                if (_data.Flags[Constants.idx_married] == true) {

                    _data.Flags[Constants.idx_married] = false;
                    _data.partners.Last().Object._data.Flags[Constants.idx_married] = false;
                    _data.partners.Last().Object._data.partners.Last().End = Environment.Now;
                    _data.partners.Last().End = Environment.Now;
                    Unmarried[Convert.ToByte(_data.Flags[Constants.idx_gender])].Push(_data.partners.Last().Object);

                    People.Events.Write(new Triple()
                    {
                        Subject = _data.partners.Last().Object._data.Id,
                        Predicate = Constants.triple_predicate_widow_of,
                        Object = _data.Id,
                        Time = Environment.Now
                    });
                }
            }

            private IEnumerable<Event> LifeCycle()
            {
                var idx = Convert.ToByte(_data.Flags[Constants.idx_gender]);
                _data.End = TimeSpan.FromDays(Environment.RandChoice(
                    Age.Source[idx,0],
                    Age.Weights[idx, 0]) * 365);
                _data.Dod = _data.Dob + _data.End;
                _data.YearDod = _data.Dod.Year;
                Global._totalAge[idx] += _data.End.Days/365;
                Environment.Process(Death());
                // Only Schedule the next process chain if the person is expected to reach maturity.
                if (_data.End.Days > 18 * 365)
                {
                    Statistics.ReachMaturity++; // indicating this person will reach maturity.
                    // Wait for adulthood.
                    yield return Environment.Timeout(TimeSpan.FromDays(18 * 365));
                    Events.Write(new Triple() { Subject = _data.Id, Predicate = Constants.triple_predicate_adult, Time = Environment.Now, Object = _data.Id });
                    // put this person in the unmarried queue
                    Unmarried[Convert.ToByte(_data.Flags[Constants.idx_gender])].Push(this);
                    // determine if this person will ever initiate a marriage process.
                    var status = Environment.RandChoice(MaritalStatus.MaritalStatusSource,
                        MaritalStatus.MaritalStatusWeights[Environment.Now.Year - 1950]);
                    if (status != Constants.marital_single)
                    {
                        // only people who do not stay single in their life cycle will be scheduled to marry.
                        var maritalAge = TimeSpan.FromDays(Environment.RandChoice(MaritalStatus.MaritalAgeSource,
                            MaritalStatus.MaritalAgeWeights[idx])) * 365;
                        // only people who do not die before the marital age are scheduled to marry.
                        if (maritalAge < _data.End)
                            Environment.Process(Marry(maritalAge));
                    }
                }
            }

            private IEnumerable<Event> Marry(TimeSpan when)
            {
                // We need to schedule a marriage now
                yield return Environment.Timeout(when);
                if (!_data.Flags[Constants.idx_married]) // Otherwise married by another person within this timespan.
                {
                    Person partner = null;
                    _data.Flags[Constants.idx_married] = true;
                    while (partner == null || partner._data.Flags[Constants.idx_married])
                    {
                        partner = Unmarried[Convert.ToByte(!_data.Flags[Constants.idx_gender])].Pop();
                    }
                    partner._data.Flags[Constants.idx_married] = true;
                    if (_data.partners == null)
                        _data.partners = new List<DateRange<Person>>();
                    _data.partners.Add(new DateRange<Person>(partner, Environment.Now));
                    if (partner._data.partners == null)
                        partner._data.partners = new List<DateRange<Person>>();
                    partner._data.partners.Add(new DateRange<Person>(this, Environment.Now));
                    Events.Write(new Triple() { 
                        Subject = _data.Id, 
                        Predicate = Constants.triple_predicate_married_to,
                        InvPredicate = Constants.triple_predicate_married_to,
                        Time = Environment.Now, 
                        Object = partner._data.Id 
                    });
                    Statistics.Couples++;

                    //Chooses the marriageDuration and the marriage eding date
                    var marriageDuration = new TimeSpan((long)Environment.RandChoice(MaritalDuration.Source,
                           MaritalDuration.Weights) * 365);
                    var marriageEndDate = Environment.Now.Add(marriageDuration);

                    //Checks if they will get children
                    if (_data.Flags[Constants.idx_gender] == Constants.gender_female && (int)SimulationAge < 49)
                    {
                        if (Environment.RandChoice(
                            Children.MotherChildSource, Children.MotherWeights[(int)SimulationAge - 18]) == 1)
                        {
                            DetermineNumberOfChildren(marriageEndDate);
                        }
                    }
                    else if((int)partner.SimulationAge < 49)
                    {
                        if (Environment.RandChoice(
                            Children.MotherChildSource, Children.MotherWeights[(int)partner.SimulationAge - 18]) == 1)
                        {
                            partner.DetermineNumberOfChildren(marriageEndDate);
                        }
                    }

                    if (Environment.Now < _data.Dod && Environment.Now < partner._data.Dod) 
                    {
                        Environment.Process(Divorcing(marriageDuration));
                    }

                }
            }

            private void DetermineNumberOfChildren(DateTime marriageDuration)
            {
                List<TimeSpan> birthSchedules = new List<TimeSpan>();
                int year = Environment.Now.Year - 1950;
                for (int i = 0;i< Environment.RandChoice(Children.SourceAmountChildren, Children.ChildAmountWeights[year]); i++)
                {
                    // ToDo: get actual timespan from probabilities labor
                    var labourSchedule = TimeSpan.FromDays((1 + i) * 2 * 365);

                    // Determines wheter the parents has not died and is within marriage duration
                    if (Environment.Now.Add(labourSchedule) > _data.Dod && Environment.Now.Add(labourSchedule) > marriageDuration)
                        break;
                    birthSchedules.Add(labourSchedule);
                }

                if (birthSchedules.Count > 0)
                {
                    Statistics.Parents++;
                    Environment.Process(ChildBirth(birthSchedules));
                }
            }

            
            public IEnumerable<Event> ChildBirth(List<TimeSpan> birthSchedules) 
            {
                foreach (var schedule in birthSchedules)
                {
                    yield return Environment.Timeout(schedule);
                    /* create new person and assign parents */
                    var child = new Person(Environment, new List<Person>() { this, this._data.partners.Last().Object });
                    //Assigns the child to the parents of the child
                    
                    
                }
            }


            private IEnumerable<Event> Divorcing(TimeSpan marriageDuration)
            {
                yield return Environment.Timeout(marriageDuration);
                //Takes away their married status
                _data.Flags[Constants.idx_married] = false;
                _data.partners.Last().Object._data.Flags[Constants.idx_married] = false;
                _data.partners.Last().Object._data.partners.Last().End = Environment.Now;
                _data.partners.Last().End = Environment.Now;
                Unmarried[Convert.ToByte(_data.Flags[Constants.idx_gender])].Push(_data.partners.Last().Object);
                Unmarried[Convert.ToByte(_data.Flags[Constants.idx_gender])].Push(this);
                //Writes that the divorce happened
                Events.Write(new Triple()
                {
                    Subject = _data.Id,
                    Predicate = Constants.triple_predicate_divorced_from,
                    InvPredicate = Constants.triple_predicate_divorced_from,
                    Time = Environment.Now,
                    Object = _data.partners.Last().Object._data.Id
                });

            }

            public double SimulationAge
            {
                get
                {
                    return (Environment.Now - _data.Dob).TotalDays / 365;
                }
                set
                {

                }
            }
        }
    }
}
