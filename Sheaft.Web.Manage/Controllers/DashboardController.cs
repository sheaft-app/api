using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Domain.Models;
using Sheaft.Application.Interop;
using Sheaft.Options;

namespace Sheaft.Manage.Controllers
{
    public class DashboardController : ManageController
    {
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IConfigurationProvider configurationProvider,
            IOptionsSnapshot<RoleOptions> roleOptions,
            ILogger<DashboardController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index(CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);

            ViewBag.Consumers = await _context.Users.OfType<Consumer>().CountAsync(c => !c.RemovedOn.HasValue, token);
            //ViewBag.Tags = await _context.Tags.CountAsync(c => !c.RemovedOn.HasValue, token);
            //ViewBag.Departments = await _context.Departments.CountAsync(token);
            //ViewBag.Regions = await _context.Regions.CountAsync(token);
            //ViewBag.Levels = await _context.Levels.CountAsync(c => !c.RemovedOn.HasValue, token);
            //ViewBag.Rewards = await _context.Rewards.CountAsync(c => !c.RemovedOn.HasValue, token);

            if (requestUser.IsImpersonating)
            {
                ViewBag.Products = await _context.Products.CountAsync(c => c.Producer.Id == requestUser.Id, token);
                ViewBag.Stores = await _context.Users.OfType<Store>().CountAsync(c => !c.RemovedOn.HasValue && c.Id == requestUser.Id, token);
                ViewBag.Producers = await _context.Users.OfType<Producer>().CountAsync(c => !c.RemovedOn.HasValue && c.Id == requestUser.Id, token);
                ViewBag.Agreements = await _context.Agreements.CountAsync(c => !c.RemovedOn.HasValue && (c.Delivery.Producer.Id == requestUser.Id || c.Store.Id == requestUser.Id), token);
                ViewBag.DeliveryModes = await _context.DeliveryModes.CountAsync(c => !c.RemovedOn.HasValue && c.Producer.Id == requestUser.Id, token);
                ViewBag.Jobs = await _context.Jobs.CountAsync(c => !c.RemovedOn.HasValue && c.User.Id == requestUser.Id, token);
                ViewBag.Returnables = await _context.Returnables.CountAsync(c => !c.RemovedOn.HasValue && c.Producer.Id == requestUser.Id, token);
                ViewBag.PurchaseOrders = await _context.PurchaseOrders.CountAsync(c => !c.RemovedOn.HasValue && (c.Vendor.Id == requestUser.Id || c.Sender.Id == requestUser.Id), token);
                ViewBag.Orders = await _context.Orders.CountAsync(c => !c.RemovedOn.HasValue && c.User.Id == requestUser.Id, token);
                ViewBag.Payins = await _context.Payins.CountAsync(c => !c.RemovedOn.HasValue && c.Author.Id == requestUser.Id, token);
                ViewBag.Transfers = await _context.Transfers.CountAsync(c => !c.RemovedOn.HasValue && c.Author.Id == requestUser.Id, token);
                ViewBag.Payouts = await _context.Payouts.CountAsync(c => !c.RemovedOn.HasValue && c.Author.Id == requestUser.Id, token);
                ViewBag.Donations = await _context.Donations.CountAsync(c => !c.RemovedOn.HasValue && c.Author.Id == requestUser.Id, token);
            }
            else
            {
                ViewBag.Products = await _context.Products.CountAsync(token);
                ViewBag.Stores = await _context.Users.OfType<Store>().CountAsync(c => !c.RemovedOn.HasValue, token);
                ViewBag.Producers = await _context.Users.OfType<Producer>().CountAsync(c => !c.RemovedOn.HasValue, token);
                ViewBag.Agreements = await _context.Agreements.CountAsync(c => !c.RemovedOn.HasValue, token);
                ViewBag.DeliveryModes = await _context.DeliveryModes.CountAsync(c => !c.RemovedOn.HasValue, token);
                ViewBag.Jobs = await _context.Jobs.CountAsync(c => !c.RemovedOn.HasValue, token);
                ViewBag.Returnables = await _context.Returnables.CountAsync(c => !c.RemovedOn.HasValue, token);
                ViewBag.PurchaseOrders = await _context.PurchaseOrders.CountAsync(c => !c.RemovedOn.HasValue, token);
                ViewBag.Orders = await _context.Orders.CountAsync(c => !c.RemovedOn.HasValue, token);
                ViewBag.Payins = await _context.Payins.CountAsync(c => !c.RemovedOn.HasValue, token);
                ViewBag.Transfers = await _context.Transfers.CountAsync(c => !c.RemovedOn.HasValue, token);
                ViewBag.Payouts = await _context.Payouts.CountAsync(c => !c.RemovedOn.HasValue, token);
                ViewBag.Donations = await _context.Donations.CountAsync(c => !c.RemovedOn.HasValue, token);
            }

            ViewBag.RequestUser = requestUser;
            return View();
        }
    }
}
