using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Sheaft.Manage.Controllers
{
    public class PurchaseOrdersController : Controller
    {
        private readonly ILogger<PurchaseOrdersController> _logger;

        public PurchaseOrdersController(ILogger<PurchaseOrdersController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
