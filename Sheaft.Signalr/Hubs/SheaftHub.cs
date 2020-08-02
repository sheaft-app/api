using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sheaft.Signalr.Controllers
{
    [Authorize]
    public class SheaftHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var roles = Context.User.Claims.Where(c => c.Type == ClaimTypes.Role);
            foreach (var role in roles)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, role.Value);
            }

            var company = Context.User.Claims.FirstOrDefault(c => c.Type == "company_id")?.Value;
            if(company != null)
                await Groups.AddToGroupAsync(Context.ConnectionId, company);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var roles = Context.User.Claims.Where(c => c.Type == ClaimTypes.Role);
            foreach(var role in roles)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, role.Value);
            }

            var company = Context.User.Claims.FirstOrDefault(c => c.Type == "company_id")?.Value;
            if (company != null)
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, company);

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
