using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Commands;
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
            var requestUser = new RequestUser("hook", _httpContextAccessor.HttpContext.TraceIdentifier);
            var identifier = ressourceId ?? resourceId;
            IBaseRequest hook = null;
            string queue = null;

            switch (EventType)
            {
                case PspEventKind.KYC_SUCCEEDED:
                    queue = SetDocumentSucceededCommand.QUEUE_NAME;
                    hook = new SetDocumentSucceededCommand(requestUser, identifier, GetExecutedOn(date));
                    break;
                case PspEventKind.KYC_FAILED:
                    queue = SetDocumentFailedCommand.QUEUE_NAME;
                    hook = new SetDocumentFailedCommand(requestUser, identifier, GetExecutedOn(date));
                    break;
                case PspEventKind.KYC_OUTDATED:
                    queue = SetDocumentOutDatedCommand.QUEUE_NAME;
                    hook = new SetDocumentOutDatedCommand(requestUser, identifier, GetExecutedOn(date));
                    break;
                case PspEventKind.KYC_VALIDATION_ASKED:
                    queue = SetDocumentValidationCommand.QUEUE_NAME;
                    hook = new SetDocumentValidationCommand(requestUser, identifier, GetExecutedOn(date));
                    break;
                case PspEventKind.UBO_DECLARATION_REFUSED:
                    queue = SetUboDeclarationRefusedCommand.QUEUE_NAME;
                    hook = new SetUboDeclarationRefusedCommand(requestUser, identifier, GetExecutedOn(date));
                    break;
                case PspEventKind.UBO_DECLARATION_VALIDATED:
                    queue = SetUboDeclarationValidatedCommand.QUEUE_NAME;
                    hook = new SetUboDeclarationValidatedCommand(requestUser, identifier, GetExecutedOn(date));
                    break;
                case PspEventKind.UBO_DECLARATION_INCOMPLETE:
                    queue = SetUboDeclarationIncompleteCommand.QUEUE_NAME;
                    hook = new SetUboDeclarationIncompleteCommand(requestUser, identifier, GetExecutedOn(date));
                    break;
                case PspEventKind.UBO_DECLARATION_VALIDATION_ASKED:
                    queue = SetUboDeclarationValidationCommand.QUEUE_NAME;
                    hook = new SetUboDeclarationValidationCommand(requestUser, identifier, GetExecutedOn(date));
                    break;
                case PspEventKind.PAYIN_NORMAL_SUCCEEDED:
                    queue = SetPayinSucceededCommand.QUEUE_NAME;
                    hook = new SetPayinSucceededCommand(requestUser, identifier, GetExecutedOn(date));
                    break;
                case PspEventKind.PAYIN_NORMAL_FAILED:
                    queue = SetPayinFailedCommand.QUEUE_NAME;
                    hook = new SetPayinFailedCommand(requestUser, identifier, GetExecutedOn(date));
                    break;
                case PspEventKind.TRANSFER_NORMAL_SUCCEEDED:
                    queue = SetTransferSucceededCommand.QUEUE_NAME;
                    hook = new SetTransferSucceededCommand(requestUser, identifier, GetExecutedOn(date));
                    break;
                case PspEventKind.TRANSFER_NORMAL_FAILED:
                    queue = SetTransferFailedCommand.QUEUE_NAME;
                    hook = new SetTransferFailedCommand(requestUser, identifier, GetExecutedOn(date));
                    break;
                case PspEventKind.PAYOUT_NORMAL_SUCCEEDED:
                    queue = SetPayoutSucceededCommand.QUEUE_NAME;
                    hook = new SetPayoutSucceededCommand(requestUser, identifier, GetExecutedOn(date));
                    break;
                case PspEventKind.PAYOUT_NORMAL_FAILED:
                    queue = SetPayoutFailedCommand.QUEUE_NAME;
                    hook = new SetPayoutFailedCommand(requestUser, identifier, GetExecutedOn(date));
                    break;
                case PspEventKind.PAYIN_REFUND_SUCCEEDED:
                    queue = SetPayinRefundSucceededCommand.QUEUE_NAME;
                    hook = new SetPayinRefundSucceededCommand(requestUser, identifier, GetExecutedOn(date));
                    break;
                case PspEventKind.PAYIN_REFUND_FAILED:
                    queue = SetPayinRefundFailedCommand.QUEUE_NAME;
                    hook = new SetPayinRefundFailedCommand(requestUser, identifier, GetExecutedOn(date));
                    break;
                case PspEventKind.TRANSFER_REFUND_SUCCEEDED:
                    queue = SetTransferRefundSucceededCommand.QUEUE_NAME;
                    hook = new SetTransferRefundSucceededCommand(requestUser, identifier, GetExecutedOn(date));
                    break;
                case PspEventKind.TRANSFER_REFUND_FAILED:
                    queue = SetTransferRefundFailedCommand.QUEUE_NAME;
                    hook = new SetTransferRefundFailedCommand(requestUser, identifier, GetExecutedOn(date));
                    break;
                default:
                    _logger.LogInformation($"{EventType:G)} is not a supported Psp EventType for resource: {identifier} executed on: {GetExecutedOn(date)}.");
                    return BadRequest();
            }

            await _queueService.ProcessCommandAsync(queue, hook, token);
            return Ok();
        }

        private DateTimeOffset GetExecutedOn(long date)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(date);
        }
    }
}
