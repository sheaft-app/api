using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Options;
using Sheaft.Domain.Enum;
using Sheaft.Web.Payment.Models;

namespace Sheaft.Web.Payment.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly PspOptions _pspOptions;
        private readonly IPspService _pspService;

        public PaymentsController(
            IOptionsSnapshot<PspOptions> pspOptions,
            IPspService pspService,
            ILogger<PaymentsController> logger)
        {
            _logger = logger;
            _pspOptions = pspOptions.Value;
            _pspService = pspService;
        }

        [HttpGet, HttpPost]
        public IActionResult Index(string transactionId, string token)
        {
            if (string.IsNullOrWhiteSpace(transactionId))
            {
                _logger.LogError($"Missing transaction id at {DateTimeOffset.UtcNow:yyyyMMddHHmmss}");
                return BadRequest("L'identifiant de transaction est requis");
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                _logger.LogError($"Missing token at {DateTimeOffset.UtcNow:yyyyMMddHHmmss}");
                return BadRequest("Le token de transaction est requis");
            }

            ViewBag.TransactionId = transactionId;
            ViewBag.Token = token;
            ViewBag.PaylineUrl = _pspOptions.PaylineUrl;

            _logger.LogInformation($"Displaying {transactionId} with token {token} at {DateTimeOffset.UtcNow:yyyyMMddHHmmss}");

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Transaction(string transactionId, CancellationToken token)
        {
            try
            {
                var payin = await _pspService.GetPayinAsync(transactionId, token);
                if (!payin.Succeeded)
                {
                    _logger.LogError(payin.Exception, $"Failed to retrieve transaction {transactionId} informations, redirecting to pending page.");
                    return RedirectPreserveMethod(_pspOptions.AppRedirectPendingUrl.Replace("{transactionId}", transactionId));
                }

                if (payin.Data.Status == TransactionStatus.Created)
                {
                    _logger.LogInformation($"Transaction {transactionId} is still in created status, redirecting to pending page.");
                    return RedirectPreserveMethod(_pspOptions.AppRedirectPendingUrl.Replace("{transactionId}", transactionId));
                }

                if (payin.Data.Status == TransactionStatus.Succeeded)
                {
                    _logger.LogInformation($"Transaction {transactionId} succeeded, redirecting to success page.");
                    return RedirectPreserveMethod(_pspOptions.AppRedirectSuccessUrl.Replace("{transactionId}", transactionId));
                }

                if (payin.Data.Status == TransactionStatus.Failed)
                {
                    _logger.LogInformation($"Transaction {transactionId} failed, redirecting to failed page.");
                    return RedirectPreserveMethod(_pspOptions.AppRedirectFailedUrl.Replace("{transactionId}", transactionId).Replace("{message}", HttpUtility.UrlEncode(payin.Data.ResultMessage)));
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"Unexpected error occured while processing transaction {transactionId} informations.");
            }

            _logger.LogInformation($"Transaction {transactionId} pending, redirecting to pending page.");
            return RedirectPreserveMethod(_pspOptions.AppRedirectPendingUrl.Replace("{transactionId}", transactionId));
        }
    }
}
