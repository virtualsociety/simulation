using SimSharp;
using System;
using System.Collections.Generic;
using Vs.Simulation.Shared;
using Vs.Simulation.Shared.Model;

namespace Vs.Simulation.Core
{

    /// <summary>
    /// People Example
    /// </summary>
    public partial class Population : ActiveObject<SimSharp.Simulation>
    {
        public static Statistics Statistics;
        public static LiveReporting.PopulationGrowth populationGrowth;
        public static EventStore Events = new EventStore();
        /// <summary>
        /// Unmarried Persons by Gender
        /// </summary>
        public static Stack<Person>[] Unmarried = new Stack<Person>[2];
        public static Stack<Person>[] Remarry = new Stack<Person>[2];
        public static List<Person> Persons = new List<Person>();
        public Process Process { get; private set; }
        public DateTime EndDate { get; }

        public Population(SimSharp.Simulation environment, DateTime endDate, Statistics statistics) : base(environment)
        {
            Statistics = statistics;
            populationGrowth = new LiveReporting.PopulationGrowth(environment.StartDate, endDate);
            Events.Write(new Triple() { Subject =0, Predicate = Constants.triple_predicate_created, Time = Environment.Now, Object = 0 });

            //Persons.Add(GenderType.Male, new Stack<Person>());
            // Persons.Add(GenderType.Female, new Stack<Person>());
            Unmarried[Constants.idx_gender_male] = new Stack<Person>();
            Unmarried[Constants.idx_gender_female] = new Stack<Person>();
            Remarry[Constants.idx_gender_male] = new Stack<Person>();
            Remarry[Constants.idx_gender_female] = new Stack<Person>();
            Warmup(0.5f);
            EndDate = endDate;
        }
    }
}
