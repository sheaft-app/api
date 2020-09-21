using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Sheaft.Application.Commands;
using Sheaft.Application.Interop;
using Sheaft.Core;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Signalr.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotifyController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _accessor;
        private readonly IHubContext<SheaftHub> _context;
        private readonly IQueueService _queuesService;

        public NotifyController(IConfiguration configuration, IHubContext<SheaftHub> context, IHttpContextAccessor accessor, IQueueService queuesService)
        {
            _queuesService = queuesService;
            _context = context;
            _accessor = accessor;
            _configuration = configuration;
        }

        public bool IsAuthorized()
        {
            if (_accessor.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues apiKey))
            {
                return apiKey.FirstOrDefault()?.Replace("apikey ", "") == _configuration.GetValue<string>("SignalR:apikey");
            }

            return false;
        }

        [HttpPost]
        [Route("notify-all/{method}")]
        public async Task<IActionResult> NotifyAll([FromRoute] string method, [FromBody] object message, CancellationToken token)
        {
            if (!IsAuthorized())
                return Unauthorized();

            await _context.Clients.All.SendAsync("event", new { Method = method, Content = message }, token);
            return Ok();
        }

        [HttpPost]
        [Route("notify-user/{userId}/{method}")]
        public async Task<IActionResult> NotifyUser([FromRoute] string userId, [FromRoute] string method, CancellationToken token)
        {
            if (!IsAuthorized())
                return Unauthorized();

            using (var reader = new StreamReader(Request.Body))
            {
                var body = await reader.ReadToEndAsync();
                await _context.Clients.User(userId).SendAsync("event", new { Method = method, UserId = userId, Content = body }, token);

                if (Guid.TryParse(userId, out Guid id))
                    await _queuesService.ProcessCommandAsync(CreateUserNotificationCommand.QUEUE_NAME, new CreateUserNotificationCommand(new RequestUser("signalr-user", HttpContext.TraceIdentifier)) { Id = id, Method = method, Content = body }, token);
            }

            return Ok();
        }

        [HttpPost]
        [Route("notify-group/{groupName}/{method}")]
        public async Task<IActionResult> NotifyGroup([FromRoute] string groupName, [FromRoute] string method, CancellationToken token)
        {
            if (!IsAuthorized())
                return Unauthorized();

            using (var reader = new StreamReader(Request.Body))
            {
                var body = await reader.ReadToEndAsync();

                await _context.Clients.Group(groupName).SendAsync("event", new { Method = method, GroupName = groupName, Content = body }, token);

                if (Guid.TryParse(groupName, out Guid id))
                    await _queuesService.ProcessCommandAsync(CreateGroupNotificationCommand.QUEUE_NAME, new CreateGroupNotificationCommand(new RequestUser("signalr-group", HttpContext.TraceIdentifier)) { Id = id, Method = method, Content = body }, token);
            }

            return Ok();
        }
    }
}