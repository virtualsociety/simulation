using System;
using Vs.Simulation.Core.Database;
using Vs.Simulation.Core.Probabilities;

namespace Vs.Simulation.Core
{
    public class PersonEntity : IIdentifiable
    {
        public long Id { get; set; }
        public DateTime DateOfBirth { get; set; }
        public GenderType Gender { get; set; }
        public LifeEvents LifeEvent { get; set; }

        public DateTime DateOfDeath { get; set; }
        public TimeSpan Lifespan { get; set; }

        public double Age
        {
            get
            {
                return Lifespan.Days / 365;
            }
            set
            {

            }
        }
    }
}
