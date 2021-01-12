using System;

namespace Vs.Simulation.Shared.Model
{
    public struct Triple
    {
        public DateTime Time;
        public long Subject;
        public long Predicate;
        public long Object;
        public long InvPredicate;
    }
}
