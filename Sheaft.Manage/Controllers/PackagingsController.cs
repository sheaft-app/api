using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Sheaft.Manage.Controllers
{
    public class PackagingsController : Controller
    {
        private readonly ILogger<PackagingsController> _logger;

        public PackagingsController(ILogger<PackagingsController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
