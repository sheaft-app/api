using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Options;
using Sheaft.Payment.Models;

namespace Sheaft.Payment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PspOptions _pspOptions;

        public HomeController(IOptionsSnapshot<PspOptions> options, ILogger<HomeController> logger)
        {
            _logger = logger;
            _pspOptions = options.Value;
        }

        [HttpGet, HttpPost]
        public IActionResult Index(string transactionId, string token)
        {
            if (string.IsNullOrWhiteSpace(transactionId))
                throw new Exception("L'identifiant de transaction est requis");

            if (string.IsNullOrWhiteSpace(token))
                throw new Exception("Le token de transaction est requis");

            ViewBag.TransactionId = transactionId;
            ViewBag.Token = token;
            ViewBag.PaylineUrl = _pspOptions.PaylineUrl;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
