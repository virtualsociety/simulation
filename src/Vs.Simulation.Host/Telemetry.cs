using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Vs.Simulation.Shared.Model;

namespace Vs.Simulation.Host
{
    internal class Telemetry : Hub
    {


        /// <summary>
        /// Sends a single triple to one client or a group of clients.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public Task SendTriple(Triple t, string group = null)
        {
            if (string.IsNullOrEmpty(group))
            {
                return Clients.Caller.SendAsync("ReceiveTriple", t);
            }
            else
            {
                return Clients.Group(group).SendAsync("ReceiveTriple", t);
            }
        }

        public Task SendStatistics(Statistics statistics, string group = null)
        {
            if (string.IsNullOrEmpty(group))
            {
                return Clients.Caller.SendAsync("ReceiveStatistics", statistics);
            }
            else
            {
                return Clients.Group(group).SendAsync("ReceiveStatistics", statistics);
            }
        }
    }
}