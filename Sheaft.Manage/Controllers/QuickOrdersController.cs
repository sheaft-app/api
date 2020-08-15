using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Sheaft.Manage.Controllers
{
    public class QuickOrdersController : Controller
    {
        private readonly ILogger<QuickOrdersController> _logger;

        public QuickOrdersController(ILogger<QuickOrdersController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
