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

            ViewBag.Consumers = await _context.Users.CountAsync(c => c.Company == null, token);
            ViewBag.Tags = await _context.Tags.CountAsync(c => !c.RemovedOn.HasValue, token);

            if (requestUser.IsImpersonating)
            {
                ViewBag.Products = await _context.Products.CountAsync(c => c.Producer.Id == requestUser.CompanyId, token);
                ViewBag.Stores = await _context.Companies.CountAsync(c => !c.RemovedOn.HasValue && c.Kind == Interop.Enums.ProfileKind.Store && c.Id == requestUser.CompanyId, token);
                ViewBag.Producers = await _context.Companies.CountAsync(c => !c.RemovedOn.HasValue && c.Kind == Interop.Enums.ProfileKind.Producer && c.Id == requestUser.CompanyId, token);
                ViewBag.Agreements = await _context.Agreements.CountAsync(c => !c.RemovedOn.HasValue && (c.Delivery.Producer.Id == requestUser.CompanyId || c.Store.Id == requestUser.CompanyId), token);
                ViewBag.DeliveryModes = await _context.DeliveryModes.CountAsync(c => !c.RemovedOn.HasValue && c.Producer.Id == requestUser.CompanyId, token);
                ViewBag.Jobs = await _context.Jobs.CountAsync(c => !c.RemovedOn.HasValue && (c.User.Id == requestUser.Id || (c.User.Company != null && c.User.Company.Id == requestUser.CompanyId)), token);
                ViewBag.Packagings = await _context.Packagings.CountAsync(c => !c.RemovedOn.HasValue && c.Producer.Id == requestUser.CompanyId, token);
                ViewBag.PurchaseOrders = await _context.PurchaseOrders.CountAsync(c => !c.RemovedOn.HasValue && (c.Vendor.Id == requestUser.CompanyId || c.Sender.Id == requestUser.Id || c.Sender.Id == requestUser.CompanyId), token);
            }
            else
            {
                ViewBag.Products = await _context.Products.CountAsync(token);
                ViewBag.Stores = await _context.Companies.CountAsync(c => !c.RemovedOn.HasValue && c.Kind == Interop.Enums.ProfileKind.Store, token);
                ViewBag.Producers = await _context.Companies.CountAsync(c => !c.RemovedOn.HasValue && c.Kind == Interop.Enums.ProfileKind.Producer, token);
                ViewBag.Agreements = await _context.Agreements.CountAsync(c => !c.RemovedOn.HasValue, token);
                ViewBag.DeliveryModes = await _context.DeliveryModes.CountAsync(c => !c.RemovedOn.HasValue, token);
                ViewBag.Jobs = await _context.Jobs.CountAsync(c => !c.RemovedOn.HasValue, token);
                ViewBag.Packagings = await _context.Packagings.CountAsync(c => !c.RemovedOn.HasValue, token);
                ViewBag.PurchaseOrders = await _context.PurchaseOrders.CountAsync(c => !c.RemovedOn.HasValue, token);
            }

            ViewBag.RequestUser = requestUser;
            return View();
        }
    }
}
