using SimSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Vs.Simulation.Core
{
    public partial class Population
    {
        public partial class Person
        {
            public IEnumerable<Event> ChildBirth(List<TimeSpan> birthSchedules)
            {
                foreach (var schedule in birthSchedules)
                {
                    yield return Environment.Timeout(schedule);
                    /* create new person and assign parents */
                    var child = new Person(Environment, new List<Person>() { this, this._data.partners.Last().Object });
                }
            }

            public void ScheduleLabor(List<TimeSpan> birthSchedules)
            {
                if (birthSchedules.Count > 0)
                {
                    Statistics.Parents++;
                    Environment.Process(ChildBirth(birthSchedules));
                }
            }
        }
    }
}
