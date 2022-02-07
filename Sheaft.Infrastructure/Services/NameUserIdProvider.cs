using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace Sheaft.Infrastructure.Services
{
    internal class NameUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
