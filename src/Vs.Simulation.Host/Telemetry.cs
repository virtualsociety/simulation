using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
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
        public async Task SendTriple(Triple t, string group = null)
        {
            if (string.IsNullOrEmpty(group))
            {
                await Clients.Caller.SendAsync("ReceiveTriple", t);
            }
            else
            {
                await Clients.Group(group).SendAsync("ReceiveTriple", t);
            }
        }

        public async Task SendDataPoints(IEnumerable<Tuple<double,double>> dataPoints, string group = null)
        {
            if (string.IsNullOrEmpty(group))
            {
                await Clients.Caller.SendAsync("ReceiveDataPoints", dataPoints);
            }
            else
            {
                await Clients.Group(group).SendAsync("ReceiveDataPoints", dataPoints);
            }
        }

        public async Task SendStatistics(Statistics statistics, string group = null)
        {
            if (string.IsNullOrEmpty(group))
            {
                await Clients.Caller.SendAsync("ReceiveStatistics", statistics);
            }
            else
            {
                await Clients.Group(group).SendAsync("ReceiveStatistics", statistics);
            }
        }
    }
}