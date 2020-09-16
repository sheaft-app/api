using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Events;
using Sheaft.Core;
using Sheaft.Interop.Enums;
using Sheaft.Services.Interop;

namespace Sheaft.Payment.Controllers
{
    public class HooksController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IQueueService _queueService;
        private readonly ILogger<HooksController> _logger;

        public HooksController(
            IQueueService queueService,
            IHttpContextAccessor httpContextAccessor,
            ILogger<HooksController> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _queueService = queueService;
            _logger = logger;
        }

        [HttpGet, HttpPost]
        public async Task<IActionResult> Notify(PspEventKind EventType, long date, CancellationToken token, string resourceId = null, string ressourceId = null)
        {
            var hook = new NewPspHookEvent(new RequestUser("hook", _httpContextAccessor.HttpContext.TraceIdentifier)) 
            {
                Kind = EventType, 
                Date = date,
                Identifier = ressourceId ?? resourceId
            };

            await _queueService.ProcessEventAsync(NewPspHookEvent.QUEUE_NAME, hook, token);
            return Ok();
        }
    }
}
