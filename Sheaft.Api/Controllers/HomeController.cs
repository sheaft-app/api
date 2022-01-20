using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace Sheaft.Api.Controllers
{
    [Authorize]
    [RequiredScope(new []{"user"})]
    public class HomeController : Controller
    {
        public IActionResult Index(string id)
        {
            return Ok("");
        }
    }
}