using System;

namespace Vs.Simulation.Terminal
{
    class Program
    {
        static void Main(string[] args)
        {
            var population = new Simulation();
            population.Simulate();
            Console.ReadLine();
        }
    }
}
