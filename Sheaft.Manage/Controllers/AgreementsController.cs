using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Sheaft.Manage.Controllers
{
    public class AgreementsController : Controller
    {
        private readonly ILogger<AgreementsController> _logger;

        public AgreementsController(ILogger<AgreementsController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
