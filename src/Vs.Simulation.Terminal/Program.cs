using Microsoft.Extensions.DependencyInjection;
using System;
using Vs.Simulation.Core;
using Vs.Simulation.Core.Infrastructure;

namespace Vs.Simulation.Terminal
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddEventLogging();
             // var provider = services.BuildServiceProvider();
            // IMPORTANT! Register our application entry point
            //services.AddTransient<PersonObject>();
            Console.WriteLine("Hello World!");

            PersonObject person = new PersonObject(null);
        }
    }
}
