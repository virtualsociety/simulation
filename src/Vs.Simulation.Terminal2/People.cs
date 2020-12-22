using SimSharp;
using System;
using System.Collections;
using System.Collections.Generic;
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
                yield return Environment.Timeout(TimeSpan.FromMinutes(Environment.RandNormal(10, 0)));
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
             // public List<Person> children;
             // public List<Person> parents;
                public List<DateRange<Person>> partners;
            }

            private static readonly TimeSpan PtMean = TimeSpan.FromDays(45*365); // Avg. age in days
            private static readonly TimeSpan PtSigma = TimeSpan.FromDays(9*365); // Sigma of age.
            public Data _data = new Data();
            public Process Process;

            public Person(SimSharp.Simulation environment) : base(environment)
            {
                _data.Flags = new BitArray(2);

                _data.Id = Global._counter++;
                Statistics.People++;
                _data.Dob = Environment.Now;
                Events.Write(new Triple() { Subject = _data.Id, Predicate = Constants.triple_predicate_child_of, Time=Environment.Now, Object = 0 });
                // Determine Gender
                _data.Flags[Constants.idx_gender] = Environment.RandChoice(Gender.Source, Gender.Weights);
                //Persons[Gender].Push(this);
                // Start the "LifeCycle"
                Process = environment.Process(LifeCycle());
            }

            private IEnumerable<Event> LifeCycle()
            {
                _data.End = TimeSpan.FromDays(Environment.RandChoice(
                    Age.Source[Convert.ToByte(_data.Flags[Constants.idx_gender]),0],
                    Age.Weights[Convert.ToByte(_data.Flags[Constants.idx_gender]), 0]) * 365);

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
                            MaritalStatus.MaritalAgeWeights[Convert.ToByte(_data.Flags[Constants.idx_gender])])) * 365;
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

                    


                    double hasChildren = 0;
                    if (_data.Flags[Constants.idx_gender] == Constants.gender_female && (int)SimulationAge < 49)
                    {
                        hasChildren = Environment.RandChoice(Children.MotherChildSource, Children.MotherWeights[(int)SimulationAge-18]);
                        if (hasChildren == 1)
                        {
                            //ChildBirth();
                        }
                    }
                    else if((int)partner.SimulationAge < 49)
                    {
                        hasChildren = Environment.RandChoice(Children.MotherChildSource, Children.MotherWeights[(int)partner.SimulationAge-18]);
                        if (hasChildren == 1)
                        {
                            //partner.ChildBirth();
                        }
                    }

                    
                    
                }

            }

            private IEnumerable<Event> Divorcing()
            {

                yield return Environment.Timeout(TimeSpan.FromDays(Environment.RandChoice(Divorce.DivorceRateSource,
                           Divorce.DivorceRateWeights)) * 365);

                if(_data.Flags == )

            }

            //public IEnumerable<Event> ChildBirth() 
            //{
            //    
            //}

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
