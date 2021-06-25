using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Sheaft.Web.Signalr.Hubs
{
    [Authorize]
    public class SheaftHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var roles = Context.User.Claims.Where(c => c.Type == ClaimTypes.Role);
            foreach (var role in roles)
                await Groups.AddToGroupAsync(Context.ConnectionId, role.Value);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var roles = Context.User.Claims.Where(c => c.Type == ClaimTypes.Role);
            foreach(var role in roles)
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, role.Value);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task UpdateUserGroupRole(string user, string role)
        {
            var roles = Context.User.Claims.Where(c => c.Type == ClaimTypes.Role);
            if (!roles.All(r => r.Value == role))
                return;

            await Groups.AddToGroupAsync(Context.ConnectionId, role);
        }
    }
}
