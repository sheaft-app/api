using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Sheaft.Manage.Controllers
{
    public class DeliveryModesController : Controller
    {
        private readonly ILogger<DeliveryModesController> _logger;

        public DeliveryModesController(ILogger<DeliveryModesController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
