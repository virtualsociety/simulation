using System.Collections.Generic;
using Vs.Simulation.Shared.Model;

namespace Vs.Simulation.Core
{
    public class EventStore
    {
        public List<Triple> _events { get; set; } = new List<Triple>();

        /// <summary>
        /// Writes one or two events.
        /// If the Inv predicate is specified, it will add a inverse of object <-> subject on that predicate (same time stamp).
        /// </summary>
        /// <param name="t"></param>
        public void Write(Triple t)
        {
            _events.Add(t);
            if (t.InvPredicate>0)
                _events.Add(new Triple() { Object = t.Subject, Predicate = t.InvPredicate, Subject = t.Object, Time = t.Time });
        }
    }
}
