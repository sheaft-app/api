using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Commands;
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Domain.Enums;

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
            var requestUser = new RequestUser("hook", _httpContextAccessor.HttpContext.TraceIdentifier);
            var identifier = ressourceId ?? resourceId;

            switch (EventType)
            {
                case PspEventKind.KYC_SUCCEEDED:
                case PspEventKind.KYC_FAILED:
                case PspEventKind.KYC_OUTDATED:
                case PspEventKind.KYC_VALIDATION_ASKED:
                    await _queueService.ProcessCommandAsync(new RefreshDocumentStatusCommand(requestUser, identifier), token);
                    break;
                case PspEventKind.UBO_DECLARATION_REFUSED:
                case PspEventKind.UBO_DECLARATION_VALIDATED:
                case PspEventKind.UBO_DECLARATION_INCOMPLETE:
                case PspEventKind.UBO_DECLARATION_VALIDATION_ASKED:
                    await _queueService.ProcessCommandAsync(new RefreshDeclarationStatusCommand(requestUser, identifier), token);
                    break;
                case PspEventKind.PAYIN_NORMAL_SUCCEEDED:
                case PspEventKind.PAYIN_NORMAL_FAILED:
                    await _queueService.ProcessCommandAsync(new RefreshPayinStatusCommand(requestUser, identifier), token);
                    break;
                case PspEventKind.TRANSFER_NORMAL_SUCCEEDED:
                case PspEventKind.TRANSFER_NORMAL_FAILED:
                    await _queueService.ProcessCommandAsync(new RefreshTransferStatusCommand(requestUser, identifier), token);
                    break;
                case PspEventKind.PAYOUT_NORMAL_SUCCEEDED:
                case PspEventKind.PAYOUT_NORMAL_FAILED:
                    await _queueService.ProcessCommandAsync(new RefreshPayoutStatusCommand(requestUser, identifier), token);
                    break;
                case PspEventKind.PAYIN_REFUND_SUCCEEDED:
                case PspEventKind.PAYIN_REFUND_FAILED:
                    await _queueService.ProcessCommandAsync(new RefreshPayinRefundStatusCommand(requestUser, identifier), token);
                    break;
                case PspEventKind.TRANSFER_REFUND_SUCCEEDED:
                case PspEventKind.TRANSFER_REFUND_FAILED:
                    await _queueService.ProcessCommandAsync(new RefreshTransferRefundStatusCommand(requestUser, identifier), token);
                    break;
                case PspEventKind.PAYOUT_REFUND_SUCCEEDED:
                case PspEventKind.PAYOUT_REFUND_FAILED:
                    await _queueService.ProcessCommandAsync(new RefreshPayoutRefundStatusCommand(requestUser, identifier), token);
                    break;
                default:
                    _logger.LogInformation($"{EventType:G)} is not a supported Psp EventType for resource: {identifier} executed on: {GetExecutedOn(date)}.");
                    return BadRequest();
            }
            
            return Ok();
        }

        private DateTimeOffset GetExecutedOn(long date)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(date);
        }
    }
}
