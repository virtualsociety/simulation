using SimSharp;
using Vs.Simulation.Core.Providers;

namespace Vs.Simulation.Core
{
    /// <summary>
    /// A person visits serveral life events, as set forward in the Person State Machine.
    /// </summary>
    public class PersonObject : ActiveObject<SimSharp.Simulation>
    {
        private readonly IEventLoggingProvider _logger;

        public PersonState State { get; set; }

        public PersonObject(SimSharp.Simulation environment) : base(environment)
        {
        }
    }
}
