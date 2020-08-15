using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sheaft.Infrastructure.Interop;

namespace Sheaft.Manage.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IAppDbContext _context;

        public DashboardController(
            IAppDbContext context,
            ILogger<DashboardController> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Products = await _context.Products.CountAsync(c => !c.RemovedOn.HasValue);
            ViewBag.Consumers = await _context.Users.CountAsync(c => !c.RemovedOn.HasValue && c.UserType == Interop.Enums.UserKind.Consumer);
            ViewBag.Stores = await _context.Companies.CountAsync(c => !c.RemovedOn.HasValue && c.Kind == Interop.Enums.ProfileKind.Store);
            ViewBag.Producers = await _context.Companies.CountAsync(c => !c.RemovedOn.HasValue && c.Kind == Interop.Enums.ProfileKind.Producer);

            return View();
        }
    }
}
