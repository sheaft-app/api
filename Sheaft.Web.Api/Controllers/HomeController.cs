using Microsoft.AspNetCore.Mvc;

namespace Sheaft.Web.Api.Controllers
{
    public class HomeController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok("");
        }
    }
}