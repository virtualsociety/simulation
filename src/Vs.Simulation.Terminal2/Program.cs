using Deedle;
using Deedle.Internal;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Threading;
using Vs.Simulation.Core.Probabilities;

namespace Vs.Simulation.Terminal2
{
    class Program
    {
        private static TimeSpan perf;
        private static DateTime startPerf;
        private static DateTime endDate;
       // private static ProgressBar progressBar = new ProgressBar();

        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();
            Console.SetCursorPosition(0, 1);
            Console.WriteLine(@"                                         ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓                
                                 ▓▓▓▓▓▓   ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓            
                                 ▓▓▓▓▓▓▓▓   ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓          
                                             ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓         
                          ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓   ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓        
                          ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓  ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓       
                      ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓   ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓      
                      ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓   ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓      
                                               ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓      
                 ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓   ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓      
                 ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓   ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓      
              ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓   ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓  
              ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓    ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓   
                                                  ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓    
                 ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓    ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓    
                 ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓    ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓     
                      ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓   ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓     
                      ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓   ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓     
                                                 ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓       
                          ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓    ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓        
                          ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓   ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓        
                               ▓▓▓▓▓▓▓▓▓▓▓▓▓   ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓          
                               ▓▓▓▓▓▓▓▓▓▓▓▓▓   ▓▓▓▓▓▓▓▓▓▓▓▓▓▓           
                                               ▓▓▓▓▓▓▓▓▓▓▓▓▓            
                                          ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓               
                                          ▓▓▓▓▓▓▓▓▓▓▓▓▓                 
                                             ▓▓▓▓▓  ");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.SetCursorPosition(0, 1);
                        Console.WriteLine(@"          
                                 ▓▓▓▓▓▓   
                                 ▓▓▓▓▓▓▓▓   
                                            
                          ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓   
                          ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓  
                      ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓  
                      ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓  
                                              
                 ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓  
                 ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓  
              ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓  
              ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓    
                                                 
                 ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ 
                 ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ 
                      ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓   
                      ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓   
                                                 
                          ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓  
                          ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓  
                               ▓▓▓▓▓▓▓▓▓▓▓▓▓   
                               ▓▓▓▓▓▓▓▓▓▓▓▓▓");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(71, 10);
            Console.WriteLine("Loading the Central Bureau of Statistics");
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(71, 12);
            Console.Write("Age Probabilities...");
            Probabilities.Age.Init();
            Console.WriteLine("Loaded");
            Console.SetCursorPosition(71, 13);
            Console.Write("ChildBirth Probabilities...");
            Probabilities.Children.Init();
            Console.WriteLine("Loaded");
            Console.SetCursorPosition(71, 14);
            Console.Write("Marriage Probabilities...");
            Probabilities.MaritalStatus.Init();
            Console.WriteLine("Loaded");

            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, 7);
            Console.WriteLine(@"                                                                 ▄▄▄▄▄▄▄  ▄     ▄▄  ▄▄ ▄▄▄▄▄▄▄  
                                                                 █ ▄▄▄ █ ▀█▄█▀ ▄ ▄ ▄▄  █ ▄▄▄ █  
                                                                 █ ███ █ ▀▀▄▀▄▄▄▄▀▀ ▄█ █ ███ █  
                                                                 █▄▄▄▄▄█ █ █ ▄▀█▀█ ▄ █ █▄▄▄▄▄█  
                                                                 ▄▄▄▄▄ ▄▄▄▄▄ █▀▄█ ▄ ▄ ▄ ▄ ▄ ▄   
                                                                  ▀  █ ▄ ▀█ ▄▄  ▀▀ █▀▀ ▀▀█   ▀  
                                                                 ▀▄▄ ▄█▄███▀█  ▀ ▀▄ ▀█    █▄▀   
                                                                 ▀▀ ▀▄█▄▀█▀█ ▀███▄▀▀▀██▀▀█▄▄ ▀  
                                                                 ▀▄▄█▀ ▄▄ ██ █▀▄▀▄▄▀█▀█▀█▀▄▄▀   
                                                                 █  ▀▄ ▄ ▀ ▄▄▄  ▄▀▄▄▀▄▀███ █ ▀  
                                                                 █ ▄▄▄ ▄▀▄▀▀█  ▀██▄ ▄███▄▄ ▄█▄  
                                                                 ▄▄▄▄▄▄▄ ██▀ ▀██▄██▀▄█ ▄ ███▀▀  
                                                                 █ ▄▄▄ █ ▄▄▀ █▀▄█▀▄ ▀█▄▄▄█ ▄▄   
                                                                 █ ███ █ █▄█▄▄  ▄█▄███▄▄▄▄███▀  
                                                                 █▄▄▄▄▄█ ██ █  ▀▀ ▄ ▄▀▀▄█▄▀▄▀
                                                                  SCAN QR FOR GITHUB PROJECT");
  
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"____   ____.__         __               .__      _________             .__        __          
\   \ /   /|__|_______/  |_ __ _______  |  |    /   _____/ ____   ____ |__| _____/  |_ ___.__.
 \   Y   / |  \_  __ \   __\  |  \__  \ |  |    \_____  \ /  _ \_/ ___\|  |/ __ \   __<   |  |
  \     /  |  ||  | \/|  | |  |  // __ \|  |__  /        (  <_> )  \___|  \  ___/|  |  \___  |
   \___/   |__||__|   |__| |____/(____  /____/ /_______  /\____/ \___  >__|\___  >__|  / ____|
                                      \/               \/        time\/machine \/ 0.1  \/");

            var startDate = new DateTime(1950, 1, 1);
            endDate = new DateTime(2020, 1, 1).AddDays(-1);
            Console.ForegroundColor = ConsoleColor.White;
            var env = new SimSharp.Simulation(new DateTime(1950,1,1),42);
            var People = new People(env);
            startPerf = DateTime.UtcNow.AddYears(-1);
            Console.SetCursorPosition(0, 18);
            Console.WriteLine(@"|     .-.
|    /   \         .-.
|   /     \       /   \       .-.     .-.     _   _
+--/-------\-----/-----\-----/---\---/---\---/-\-/-\/\/---
| /         \   /       \   /     '-'     '-'
|/           '-'         '-'");
            Timer t = new Timer(Reporter, env, 1000, 250);
            env.Run(endDate);
            perf = DateTime.UtcNow.AddYears(-1) - startPerf;
          //  var q = from p in People.Persons[GenderType.Male] where p.IsMarried select p;
           // Console.WriteLine();
            t.Dispose();
            Reporter(env);
            Console.SetCursorPosition(0, 26);
            Console.CursorSize = 100;
            Console.CursorVisible = true;
            Console.Write("Saving Triple Event Store...");
            Frame.FromRecords(People.Events._events).SaveCsv(@"./events.csv");
            var msg = "Done.. press any key to exit.";
            for (int i = 0; i < msg.Length; i++)
            {
                Thread.Sleep(50);
                Console.Write(msg[i]);
            }
            Console.ReadKey();
        }

        private static void Reporter(object state)
        {
            var env = (SimSharp.Simulation)state;
            perf = DateTime.UtcNow.AddYears(-1) - startPerf;
            Console.SetCursorPosition(0,6);
            Console.WriteLine($"         People: {Statistics.People.ToString("#,#", CultureInfo.InvariantCulture)}");
            Console.WriteLine($" Reach Maturity: {((double)Statistics.ReachMaturity / (double)Statistics.People).ToString("P")}");
            Console.WriteLine($"        Couples: {Statistics.Couples}");
            Console.WriteLine($"        Couples: {Statistics.Children}");
            Console.WriteLine($"   Stack Errors: {Statistics.StackErrors}");
            Console.WriteLine($"     Events/sec: {((int)(env.ProcessedEvents / perf.TotalSeconds)).ToString("#,#", CultureInfo.InvariantCulture)}");
            Console.WriteLine($"   Total Events: {env.ProcessedEvents.ToString("#,#", CultureInfo.InvariantCulture)}");
            Console.WriteLine($"       Run Time: {perf.ToString("g")}");
            Console.WriteLine($"   Current Time: {env.StartDate} - {env.Now}");
            Process proc = Process.GetCurrentProcess();
            Console.WriteLine($"   Memory Usage: {((float)proc.PrivateMemorySize64)/1024/1024/1024} GB  ");
        }
    }
}
