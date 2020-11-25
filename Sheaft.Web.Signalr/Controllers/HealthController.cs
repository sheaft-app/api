using Microsoft.AspNetCore.Mvc;

namespace Sheaft.Web.Signalr.Controllers
{
    public class HealthController : Controller
    {
        public IActionResult Livez()
        {
            return Ok("OK");
        }

        public IActionResult Readyz()
        {
            return Ok("OK");
        }
    }
}