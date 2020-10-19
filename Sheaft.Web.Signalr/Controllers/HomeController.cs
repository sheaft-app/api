using Microsoft.AspNetCore.Mvc;

namespace Sheaft.Web.Signalr.Controllers
{
    public class HomeController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok("");
        }
    }
}