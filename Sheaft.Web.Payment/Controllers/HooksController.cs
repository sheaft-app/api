using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Commands;
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Domain.Enums;

namespace Sheaft.Web.Payment.Controllers
{
    public class HooksController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<HooksController> _logger;
        private readonly ISheaftMediatr _sheaftMediatr;

        public HooksController(
            ISheaftMediatr sheaftMediatr,
            IHttpContextAccessor httpContextAccessor,
            ILogger<HooksController> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _sheaftMediatr = sheaftMediatr;
            _logger = logger;
        }

        [HttpGet, HttpPost]
        public IActionResult Notify(PspEventKind EventType, long date, string resourceId = null, string ressourceId = null)
        {
            var requestUser = new RequestUser("hook", _httpContextAccessor.HttpContext.TraceIdentifier);
            var identifier = ressourceId ?? resourceId;

            switch (EventType)
            {
                case PspEventKind.KYC_SUCCEEDED:
                case PspEventKind.KYC_FAILED:
                case PspEventKind.KYC_OUTDATED:
                case PspEventKind.KYC_VALIDATION_ASKED:
                    _sheaftMediatr.Post(new RefreshDocumentStatusCommand(requestUser, identifier));
                    break;
                case PspEventKind.UBO_DECLARATION_REFUSED:
                case PspEventKind.UBO_DECLARATION_VALIDATED:
                case PspEventKind.UBO_DECLARATION_INCOMPLETE:
                case PspEventKind.UBO_DECLARATION_VALIDATION_ASKED:
                    _sheaftMediatr.Post(new RefreshDeclarationStatusCommand(requestUser, identifier));
                    break;
                case PspEventKind.PAYIN_NORMAL_SUCCEEDED:
                case PspEventKind.PAYIN_NORMAL_FAILED:
                    _sheaftMediatr.Post(new RefreshPayinStatusCommand(requestUser, identifier));
                    break;
                case PspEventKind.TRANSFER_NORMAL_SUCCEEDED:
                case PspEventKind.TRANSFER_NORMAL_FAILED:
                    _sheaftMediatr.Post(new RefreshTransferStatusCommand(requestUser, identifier));
                    break;
                case PspEventKind.PAYOUT_NORMAL_SUCCEEDED:
                case PspEventKind.PAYOUT_NORMAL_FAILED:
                    _sheaftMediatr.Post(new RefreshPayoutStatusCommand(requestUser, identifier));
                    break;
                case PspEventKind.PAYIN_REFUND_SUCCEEDED:
                case PspEventKind.PAYIN_REFUND_FAILED:
                    _sheaftMediatr.Post(new RefreshPayinRefundStatusCommand(requestUser, identifier));
                    break;
                case PspEventKind.USER_KYC_LIGHT:
                case PspEventKind.USER_KYC_REGULAR:
                    _sheaftMediatr.Post(new RefreshLegalValidationCommand(requestUser, identifier));
                    break;                    
                case PspEventKind.PREAUTHORIZATION_PAYMENT_CANCELED:
                case PspEventKind.PREAUTHORIZATION_PAYMENT_EXPIRED:
                case PspEventKind.PREAUTHORIZATION_PAYMENT_VALIDATED:
                case PspEventKind.PREAUTHORIZATION_PAYMENT_WAITING:
                    _sheaftMediatr.Post(new RefreshPreAuthorizationStatusCommand(requestUser, identifier));
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
