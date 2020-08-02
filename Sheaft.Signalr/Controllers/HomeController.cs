using Microsoft.AspNetCore.Mvc;

namespace Sheaft.Signalr.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        public IActionResult Home()
        {
            return Ok();
        }
    }
}