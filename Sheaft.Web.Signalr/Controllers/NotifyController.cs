using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Domain;
using Sheaft.Mediatr.Notification.Commands;
using Sheaft.Web.Signalr.Hubs;

namespace Sheaft.Web.Signalr.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotifyController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _accessor;
        private readonly IHubContext<SheaftHub> _context;
        private readonly ISheaftMediatr _sheaftMediatr;

        public NotifyController(IConfiguration configuration, IHubContext<SheaftHub> context,
            IHttpContextAccessor accessor, ISheaftMediatr sheaftMediatr)
        {
            _sheaftMediatr = sheaftMediatr;
            _context = context;
            _accessor = accessor;
            _configuration = configuration;
        }

        public bool IsAuthorized()
        {
            if (_accessor.HttpContext == null)
                return false;

            if (_accessor.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues apiKey))
            {
                return apiKey.FirstOrDefault()?.Replace("apikey ", "") ==
                       _configuration.GetValue<string>("SignalR:apikey");
            }

            return false;
        }

        [HttpPost]
        [Route("notify-all/{method}")]
        public async Task<IActionResult> NotifyAll([FromRoute] string method, [FromBody] object message,
            CancellationToken token)
        {
            if (!IsAuthorized())
                return Unauthorized();

            await _context.Clients.All.SendAsync("event", new {Method = method, Content = message}, token);
            return Ok();
        }

        [HttpPost]
        [Route("notify-user/{userId}/{method}")]
        public async Task<IActionResult> NotifyUser([FromRoute] string userId, [FromRoute] string method, CancellationToken token)
        {
            if (!IsAuthorized())
                return Unauthorized();

            if (!Guid.TryParse(userId, out Guid id))
                return BadRequest("Invalid userId (Guid format expected)");
            
            var body = string.Empty; 
            using (var reader = new StreamReader(Request.Body)) 
            { 
                body = await reader.ReadToEndAsync(); 
                await _context.Clients.User(id.ToString("N")).SendAsync("event", new { Method = method, UserId = userId, Content = body }, token); 
            } 
            
            _sheaftMediatr.Post(
                new CreateUserNotificationCommand(new RequestUser("signalr-user", HttpContext.TraceIdentifier))
                    {UserId = id, Method = method, Content = JsonConvert.SerializeObject(body)});
            
            return Ok();
        }

        [HttpPost]
        [Route("notify-group/{groupName}/{method}")]
        public async Task<IActionResult> NotifyGroup([FromRoute] string groupName, [FromRoute] string method, CancellationToken token)
        {
            if (!IsAuthorized())
                return Unauthorized();

            if (!Guid.TryParse(groupName, out Guid id))
                return BadRequest("Invalid groupName (Guid format expected)");
            
            var body = string.Empty; 
            using (var reader = new StreamReader(Request.Body)) 
            { 
                body = await reader.ReadToEndAsync(); 
                await _context.Clients.Group(groupName).SendAsync("event", new { Method = method, GroupName = groupName, Content = body }, token); 
            } 

            _sheaftMediatr.Post(
                new CreateGroupNotificationCommand(new RequestUser("signalr-group", HttpContext.TraceIdentifier))
                    {GroupId = id, Method = method, Content = JsonConvert.SerializeObject(body)});
            return Ok();
        }
    }
}