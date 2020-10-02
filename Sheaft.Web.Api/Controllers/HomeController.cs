using Microsoft.AspNetCore.Mvc;

namespace Sheaft.Web.Api.Controllers
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