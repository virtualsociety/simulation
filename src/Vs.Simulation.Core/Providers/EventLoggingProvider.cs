namespace Vs.Simulation.Core.Providers
{
    public class EventLoggingProvider : IEventLoggingProvider
    {
        public string Name { get; set; }
    }

    public interface IEventLoggingProvider
    {
        public string Name { get; set; }
    }
}
