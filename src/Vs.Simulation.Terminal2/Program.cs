using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using Vs.Simulation.Core.Probabilities;
using Vs.Simulation.Core;
using Vs.Simulation.Shared.Model;
using Vs.Simulation.Shared;
using System.Threading.Tasks;

namespace Vs.Simulation.Terminal
{
    class Program
    {
        private static TimeSpan perf;
        private static DateTime startPerf;
        private static DateTime endDate;
        private static Statistics Statistics = new Statistics();
        static Timer t;
        static SimSharp.Simulation env;

        // private static ProgressBar progressBar = new ProgressBar();

        static async Task Main(string[] args)
        {
            Host.Program.RunServer(args);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();
            Console.SetCursorPosition(0, 1);
            Console.WriteLine(AsciiLogo.VSLogo);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(0, 1);
            Console.WriteLine(AsciiLogo.VSLogo2);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(71, 10);
            Console.WriteLine("Loading the Central Bureau of Statistics");
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(71, 12);
            Console.Write("Marriage Probabilities...");
            MaritalStatusProbability.Init();
            Console.WriteLine("Loaded");
            Console.SetCursorPosition(71, 13);
            Console.Write("ChildBirth Probabilities...");
            ChildrenProbability.Init();
            Console.WriteLine("Loaded");
            Console.SetCursorPosition(71, 14);
            Console.Write("Age Probabilities...");
            AgeProbability.Init();
            Console.WriteLine("Loaded");
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, 7);
            Console.WriteLine(AsciiLogo.QR);
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(AsciiLogo.VSLogo3);
            var startDate = new DateTime(1950, 1, 1);
            endDate = new DateTime(2020, 1, 1).AddDays(-1);
            Console.ForegroundColor = ConsoleColor.White;
            env = new SimSharp.Simulation(new DateTime(1950, 1, 1), 42);
            var people = new Population(env, endDate, Statistics);
            startPerf = DateTime.UtcNow.AddYears(-1);
            Console.SetCursorPosition(0, 18);
            Console.WriteLine(AsciiLogo.Heartbeat);
            t = new Timer(Reporter, env, 1000, 250);
            env.RunFinished += Env_RunFinished;
            env.Run(endDate);
           // Console.Read();
        }

        private static void Env_RunFinished(object sender, EventArgs e)
        {
            perf = DateTime.UtcNow.AddYears(-1) - startPerf;
            //  var q = from p in People.Persons[GenderType.Male] where p.IsMarried select p;
            // Console.WriteLine();
            t.Dispose();
            Reporter(env);
            Console.SetCursorPosition(0, 26);
            Console.CursorSize = 100;
            //Console.Write("Saving Triple Event Store...");
            //Frame.FromRecords(People.Events._events).SaveCsv(@"./events.csv");
            Exports();
            Console.CursorVisible = true;
            var msg = "Done.. press any key to exit.";
            for (int i = 0; i < msg.Length; i++)
            {
                Thread.Sleep(50);
                Console.Write(msg[i]);
            }
            Console.ReadKey();
        }

        private static void Exports()
        {
            Population.populationGrowth.ToCsv(@"./population-growth.csv");
        }

        private static void Reporter(object state)
        {
            var env = (SimSharp.Simulation)state;
            perf = DateTime.UtcNow.AddYears(-1) - startPerf;
            Console.SetCursorPosition(0, 6);
            Console.WriteLine($"         People: {(Statistics.People[0] + Statistics.People[1]).ToString("#,#", CultureInfo.InvariantCulture)}");
            Console.WriteLine($" Reach Maturity: {(Statistics.ReachMaturity / (double)(Statistics.People[0] + Statistics.People[1])).ToString("P")}");
            Console.WriteLine($"        Couples: {Statistics.Couples}");
            Console.WriteLine($"        Parents: {(Statistics.Parents / (double)Statistics.Couples).ToString("P")}");
            Console.WriteLine($"       Children: {Statistics.Children}");
            Console.WriteLine($"   Stack Errors: {Statistics.StackErrors}");
            Console.WriteLine($"     Events/sec: {((int)(env.ProcessedEvents / perf.TotalSeconds)).ToString("#,#", CultureInfo.InvariantCulture)}");
            Console.WriteLine($"   Total Events: {env.ProcessedEvents.ToString("#,#", CultureInfo.InvariantCulture)}");
            Console.WriteLine($"       Run Time: {perf.ToString("g")}");
            Console.WriteLine($"   Current Time: {env.StartDate} - {env.Now}");
            Process proc = Process.GetCurrentProcess();
            Console.WriteLine($"   Memory Usage: {(float)proc.PrivateMemorySize64 / 1024 / 1024 / 1024} GB  ");

            Console.SetCursorPosition(28, 6);
            Console.WriteLine($" Avg.age Female: {Statistics.AvgAgeFemale.ToString("0.00", CultureInfo.InvariantCulture)} / {Statistics.People[Constants.idx_gender_female]}");
            Console.SetCursorPosition(28, 7);
            Console.WriteLine($" Avg.age   Male: {Statistics.AvgAgeMale.ToString("0.00", CultureInfo.InvariantCulture)} / {Statistics.People[Constants.idx_gender_male]}");
            Console.SetCursorPosition(28, 8);
            Console.WriteLine($"      Ratio F/M: 100/{(int)((double)Statistics.People[Constants.idx_gender_male] / Statistics.People[Constants.idx_gender_female] * 100)}");
            Console.SetCursorPosition(28, 9);
            Console.WriteLine($"         Deaths: {Statistics.Deaths}");
            Console.SetCursorPosition(28, 10);
            Console.WriteLine($"People (Living): {(Statistics.People[0] + Statistics.People[1] - Statistics.Deaths).ToString("#,#", CultureInfo.InvariantCulture)}");
            Console.SetCursorPosition(28, 11);
            Console.WriteLine($"      Remarried: {Statistics.Remarried}");
        }
    }
}
