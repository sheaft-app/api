using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Security.Claims;

namespace Sheaft.Web.Signalr
{
    public partial class Startup
    {
        public class NameUserIdProvider : IUserIdProvider
        {
            public string GetUserId(HubConnectionContext connection)
            {
                var userId = connection.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                return userId;
            }
        }
    }
}
