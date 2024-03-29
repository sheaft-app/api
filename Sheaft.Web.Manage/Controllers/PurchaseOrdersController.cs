﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core.Exceptions;
using Sheaft.Core.Options;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.PurchaseOrder.Commands;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Controllers
{
    public class PurchaseOrdersController : ManageController
    {
        public PurchaseOrdersController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider)
            : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken token, PurchaseOrderStatus? status = null,
            int page = 0, int take = 50)
        {
            if (page < 0)
                page = 0;

            if (take > 100)
                take = 100;

            var query = _context.PurchaseOrders.AsNoTracking();

            var requestUser = await GetRequestUserAsync(token);
            if (requestUser.IsImpersonating())
            {
                if (requestUser.IsInRole(_roleOptions.Producer.Value))
                    query = query.Where(p => p.ProducerId == requestUser.Id);
                else
                    query = query.Where(p => p.ClientId == requestUser.Id);
            }

            if (status != null)
                query = query.Where(p => p.Status == status);

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<ShortPurchaseOrderViewModel>(_configurationProvider)
                .ToListAsync(token);

            ViewBag.Page = page;
            ViewBag.Take = take;
            ViewBag.Status = status;

            return View(entities);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUserAsync(token);
            if (!requestUser.IsImpersonating())
                return RedirectToAction("Impersonate", "Account");

            var entity = await _context.PurchaseOrders
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<PurchaseOrderViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw SheaftException.NotFound();

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUserAsync(token);
            if (!requestUser.IsImpersonating() || !requestUser.IsInRole("PRODUCER"))
                return RedirectToAction("Impersonate", "Account");

            var result = await _mediatr.Process(new AcceptPurchaseOrderCommand(requestUser)
            {
                PurchaseOrderId = id,
                SkipNotification = true,
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Refuse(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUserAsync(token);
            if (!requestUser.IsImpersonating() || !requestUser.IsInRole("PRODUCER"))
                return RedirectToAction("Impersonate", "Account");

            var result = await _mediatr.Process(new RefusePurchaseOrderCommand(requestUser)
            {
                PurchaseOrderId = id,
                SkipNotification = true,
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUserAsync(token);
            if (!requestUser.IsImpersonating() || !requestUser.IsInRole("PRODUCER") || !requestUser.IsInRole("CONSUMER"))
                return RedirectToAction("Impersonate", "Account");

            var result = await _mediatr.Process(new CancelPurchaseOrderCommand(requestUser)
            {
                PurchaseOrderId = id,
                SkipNotification = true,
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Process(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUserAsync(token);
            if (!requestUser.IsImpersonating() || !requestUser.IsInRole("PRODUCER"))
                return RedirectToAction("Impersonate", "Account");

            var result = await _mediatr.Process(new ProcessPurchaseOrderCommand(requestUser)
            {
                PurchaseOrderId = id,
                SkipNotification = true,
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUserAsync(token);
            if (!requestUser.IsImpersonating() || !requestUser.IsInRole("PRODUCER"))
                return RedirectToAction("Impersonate", "Account");

            var result = await _mediatr.Process(new CompletePurchaseOrderCommand(requestUser)
            {
                PurchaseOrderId = id,
                SkipNotification = true,
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deliver(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUserAsync(token);
            if (!requestUser.IsImpersonating() || !requestUser.IsInRole("PRODUCER"))
                return RedirectToAction("Impersonate", "Account");

            var result = await _mediatr.Process(new DeliverPurchaseOrderCommand(requestUser)
            {
                PurchaseOrderId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new RestorePurchaseOrderCommand(await GetRequestUserAsync(token))
            {
                PurchaseOrderId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new DeletePurchaseOrderCommand(await GetRequestUserAsync(token))
            {
                PurchaseOrderId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Index");
        }
    }
}