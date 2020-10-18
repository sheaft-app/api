using Microsoft.AspNetCore.Mvc;

namespace Sheaft.Web.Payment.Controllers
{
    public class HomeController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok("");
        }
    }
}
