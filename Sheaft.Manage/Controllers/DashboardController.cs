using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Infrastructure.Interop;
using Sheaft.Options;

namespace Sheaft.Manage.Controllers
{
    public class DashboardController : ManageController
    {
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(
            IAppDbContext context,
            IMapper mapper,
            IMediator mediatr,
            IConfigurationProvider configurationProvider,
            IOptionsSnapshot<RoleOptions> roleOptions,
            ILogger<DashboardController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index(CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);

            if (requestUser.IsImpersonating)
            {
                ViewBag.Products = await _context.Products.CountAsync(c => c.Producer.Id == requestUser.CompanyId, token);
                ViewBag.Consumers = 0;
                ViewBag.Stores = await _context.Companies.CountAsync(c => !c.RemovedOn.HasValue && c.Kind == Interop.Enums.ProfileKind.Store && c.Id == requestUser.CompanyId, token);
                ViewBag.Producers = await _context.Companies.CountAsync(c => !c.RemovedOn.HasValue && c.Kind == Interop.Enums.ProfileKind.Producer && c.Id == requestUser.CompanyId, token);
            }
            else
            {
                ViewBag.Products = await _context.Products.CountAsync(token);
                ViewBag.Consumers = await _context.Users.CountAsync(c => c.UserType == Interop.Enums.UserKind.Consumer, token);
                ViewBag.Stores = await _context.Companies.CountAsync(c => !c.RemovedOn.HasValue && c.Kind == Interop.Enums.ProfileKind.Store, token);
                ViewBag.Producers = await _context.Companies.CountAsync(c => !c.RemovedOn.HasValue && c.Kind == Interop.Enums.ProfileKind.Producer, token);
            }


            return View();
        }
    }
}
