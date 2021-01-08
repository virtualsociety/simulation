using System;

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
}
