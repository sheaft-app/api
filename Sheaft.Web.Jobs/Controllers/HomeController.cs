using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Options;

namespace Sheaft.Web.Jobs.Controllers
{
    public class HomeController : Controller
    {
        private readonly RoleOptions _roles;

        public HomeController(IOptionsSnapshot<RoleOptions> roles)
        {
            _roles = roles.Value;
        }
        
        [AllowAnonymous]
        public IActionResult Index()
        {
            if(User.Identity is {IsAuthenticated: true} && User.IsInRole(_roles.Admin.Value) || User.IsInRole(_roles.Support.Value))
                return Redirect("/hangfire");
            
            return Ok("OK");
        }
    }
}
