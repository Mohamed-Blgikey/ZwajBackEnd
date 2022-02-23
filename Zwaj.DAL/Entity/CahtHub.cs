using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zwaj.DAL.Entity
{
    public class CahtHub : Hub
    {
        public async void refresh()
        {
            await Clients.All.SendAsync("refresh");
        }
    }
}
