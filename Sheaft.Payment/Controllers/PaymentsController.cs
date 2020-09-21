using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Options;
using Sheaft.Payment.Models;

namespace Sheaft.Payment.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly PspOptions _pspOptions;

        public PaymentsController(
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<PaymentsController> logger)
        {
            _logger = logger;
            _pspOptions = pspOptions.Value;
        }

        [HttpGet, HttpPost]
        public IActionResult Index(string transactionId, string token)
        {
            if (string.IsNullOrWhiteSpace(transactionId))
            {
                _logger.LogError($"Missing transaction id at {DateTimeOffset.UtcNow:yyyyMMddHHmmss}");
                throw new Exception("L'identifiant de transaction est requis");
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                _logger.LogError($"Missing token at {DateTimeOffset.UtcNow:yyyyMMddHHmmss}");
                throw new Exception("Le token de transaction est requis");
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

        public IActionResult Transaction(string transactionId)
        {
            return RedirectPreserveMethod(_pspOptions.AppRedirectUrl.Replace("{transactionId}", transactionId));
        }
    }
}
