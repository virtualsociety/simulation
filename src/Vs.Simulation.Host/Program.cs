using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Vs.Simulation.Host
{
    public static class Program
    {
        public static void RunServer(string[] args)
        {
            CreateWebHostBuilder(args).Build().Start();
        }

        public static void Main(string[] args)
        {
           
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => 
            {
                endpoints.MapHub<Telemetry>("/telemetry");
            });
        }

        public IConfiguration Configuration { get; }
    }
}
