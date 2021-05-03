﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Domain;
using Sheaft.Options;

namespace Sheaft.Web.Manage.Controllers
{
    public class DashboardController : ManageController
    {
        public DashboardController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IConfigurationProvider configurationProvider,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
        }

        public async Task<IActionResult> Index(CancellationToken token)
        {
            var requestUser = await GetRequestUserAsync(token);

            ViewBag.Consumers = await _context.Users.OfType<Consumer>().CountAsync(c => !c.RemovedOn.HasValue, token);
            ViewBag.Tags = await _context.Tags.CountAsync(c => !c.RemovedOn.HasValue, token);
            ViewBag.Departments = await _context.Departments.CountAsync(token);
            ViewBag.Regions = await _context.Regions.CountAsync(token);
            //ViewBag.Levels = await _context.Levels.CountAsync(c => !c.RemovedOn.HasValue, token);
            //ViewBag.Rewards = await _context.Rewards.CountAsync(c => !c.RemovedOn.HasValue, token);

            if (requestUser.IsImpersonating())
            {
                ViewBag.Products = await _context.Products.CountAsync(c => c.ProducerId == requestUser.Id, token);
                ViewBag.Stores = await _context.Users.OfType<Store>()
                    .CountAsync(c => !c.RemovedOn.HasValue && c.Id == requestUser.Id, token);
                ViewBag.Producers = await _context.Users.OfType<Producer>()
                    .CountAsync(c => !c.RemovedOn.HasValue && c.Id == requestUser.Id, token);
                ViewBag.Agreements = await _context.Agreements.CountAsync(
                    c => !c.RemovedOn.HasValue &&
                         (c.ProducerId == requestUser.Id || c.StoreId == requestUser.Id), token);
                ViewBag.DeliveryModes =
                    await _context.DeliveryModes.CountAsync(
                        c => !c.RemovedOn.HasValue && c.ProducerId == requestUser.Id, token);
                ViewBag.Jobs =
                    await _context.Jobs.CountAsync(c => !c.RemovedOn.HasValue && c.UserId == requestUser.Id, token);
                ViewBag.Returnables =
                    await _context.Returnables.CountAsync(c => !c.RemovedOn.HasValue && c.ProducerId == requestUser.Id,
                        token);
                ViewBag.PurchaseOrders = await _context.PurchaseOrders.CountAsync(
                    c => !c.RemovedOn.HasValue && (c.ProducerId == requestUser.Id || c.ClientId == requestUser.Id),
                    token);
                ViewBag.Orders =
                    await _context.Orders.CountAsync(c => !c.RemovedOn.HasValue && c.UserId == requestUser.Id, token);
                ViewBag.Payins =
                    await _context.Payins.CountAsync(c => !c.RemovedOn.HasValue && c.AuthorId == requestUser.Id,
                        token);
                ViewBag.Transfers =
                    await _context.Transfers.CountAsync(c => !c.RemovedOn.HasValue && c.AuthorId == requestUser.Id,
                        token);
                ViewBag.Payouts =
                    await _context.Payouts.CountAsync(c => !c.RemovedOn.HasValue && c.AuthorId == requestUser.Id,
                        token);
                ViewBag.Donations =
                    await _context.Donations.CountAsync(c => !c.RemovedOn.HasValue && c.AuthorId == requestUser.Id,
                        token);
                ViewBag.Withholdings =
                    await _context.Withholdings.CountAsync(c => !c.RemovedOn.HasValue && c.AuthorId == requestUser.Id,
                        token);
            }
            else
            {
                ViewBag.Products = await _context.Products.CountAsync(token);
                ViewBag.Stores = await _context.Users.OfType<Store>().CountAsync(c => !c.RemovedOn.HasValue, token);
                ViewBag.Producers =
                    await _context.Users.OfType<Producer>().CountAsync(c => !c.RemovedOn.HasValue, token);
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
                ViewBag.Withholdings = await _context.Withholdings.CountAsync(c => !c.RemovedOn.HasValue, token);
            }

            ViewBag.RequestUser = requestUser;
            return View();
        }
    }
}