using Microsoft.AspNetCore.Mvc;

namespace Sheaft.Api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(string id)
        {
            return Ok("");
        }
    }
}