using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace Sheaft.Infrastructure.Providers
{
    internal class NameUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            var userId = connection.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            return userId;
        }
    }
}
