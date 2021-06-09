using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Enum;
using Sheaft.Options;
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
                _logger.LogError($"Missing preAuthorization id at {DateTimeOffset.UtcNow:yyyyMMddHHmmss}");
                return BadRequest("L'identifiant de preAuthorization est requis");
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                _logger.LogError($"Missing token at {DateTimeOffset.UtcNow:yyyyMMddHHmmss}");
                return BadRequest("Le token de preAuthorization est requis");
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
        
        public async Task<IActionResult> Transaction(string preAuthorizationId, CancellationToken token)
        {
            try
            {
                var preAuthorization = await _pspService.GetPreAuthorizationAsync(preAuthorizationId, token);
                if (!preAuthorization.Succeeded)
                {
                    _logger.LogError(preAuthorization.Exception, $"Failed to retrieve preAuthorization {preAuthorizationId} informations, redirecting to pending page.");
                    return RedirectPreserveMethod(_pspOptions.AppRedirectPendingUrl.Replace("{transactionId}", preAuthorizationId));
                }

                if (preAuthorization.Data.Status == PreAuthorizationStatus.Created)
                {
                    _logger.LogInformation($"PreAuthorization {preAuthorizationId} is still in created status, redirecting to pending page.");
                    return RedirectPreserveMethod(_pspOptions.AppRedirectPendingUrl.Replace("{transactionId}", preAuthorizationId));
                }

                if (preAuthorization.Data.Status == PreAuthorizationStatus.Succeeded)
                {
                    _logger.LogInformation($"PreAuthorization {preAuthorizationId} succeeded, redirecting to success page.");
                    return RedirectPreserveMethod(_pspOptions.AppRedirectSuccessUrl.Replace("{transactionId}", preAuthorizationId));
                }

                if (preAuthorization.Data.Status == PreAuthorizationStatus.Failed)
                {
                    _logger.LogInformation($"PreAuthorization {preAuthorizationId} failed, redirecting to failed page.");
                    return RedirectPreserveMethod(_pspOptions.AppRedirectFailedUrl.Replace("{transactionId}", preAuthorizationId).Replace("{message}", HttpUtility.UrlEncode(preAuthorization.Data.ResultMessage)));
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"Unexpected error occured while processing preAuthorization {preAuthorizationId} informations.");
            }

            _logger.LogInformation($"PreAuthorization {preAuthorizationId} pending, redirecting to pending page.");
            return RedirectPreserveMethod(_pspOptions.AppRedirectPendingUrl.Replace("{transactionId}", preAuthorizationId));
        }
    }
}
