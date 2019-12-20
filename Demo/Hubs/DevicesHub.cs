using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Hubs
{
    public class DevicesHub : Hub
    {
        public async Task SendMessage(string dev, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", dev, message);
        }
    }
}
