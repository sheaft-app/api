using Microsoft.AspNetCore.Mvc;

namespace Sheaft.Web.Signalr.Controllers
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