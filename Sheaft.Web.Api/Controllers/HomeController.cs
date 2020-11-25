using Microsoft.AspNetCore.Mvc;

namespace Sheaft.Web.Api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Ok("");
        }
    }
}