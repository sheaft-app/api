using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models.ViewModels;
using Sheaft.Application.Common.Options;
using Sheaft.Application.PurchaseOrder.Commands;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Web.Manage.Controllers
{
    public class PurchaseOrdersController : ManageController
    {
        private readonly ILogger<PurchaseOrdersController> _logger;

        public PurchaseOrdersController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider,
            ILogger<PurchaseOrdersController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken token, PurchaseOrderStatus? status = null, int page = 0, int take = 50)
        {
            if (page < 0)
                page = 0;

            if (take > 100)
                take = 100;

            var query = _context.PurchaseOrders.AsNoTracking();

            var requestUser = await GetRequestUser(token);
            if (requestUser.IsImpersonating)
            {
                if (requestUser.IsInRole(_roleOptions.Producer.Value))
                    query = query.Where(p => p.Vendor.Id == requestUser.Id);
                else
                    query = query.Where(p => p.Sender.Id == requestUser.Id);
            }

            if (status != null)
                query = query.Where(p => p.Status == status);

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<PurchaseOrderViewModel>(_configurationProvider)
                .ToListAsync(token);

            ViewBag.Page = page;
            ViewBag.Take = take;
            ViewBag.Status = status;

            return View(entities);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            if (!requestUser.IsImpersonating)
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
            var requestUser = await GetRequestUser(token);
            if (!requestUser.IsImpersonating || !requestUser.IsInRole("PRODUCER"))
                return RedirectToAction("Impersonate", "Account");

            var result = await _mediatr.Process(new AcceptPurchaseOrderCommand(requestUser)
            {
                Id = id,
                SkipNotification = true,
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Refuse(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            if (!requestUser.IsImpersonating || !requestUser.IsInRole("PRODUCER"))
                return RedirectToAction("Impersonate", "Account");

            var result = await _mediatr.Process(new RefusePurchaseOrderCommand(requestUser)
            {
                Id = id,
                SkipNotification = true,
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            if (!requestUser.IsImpersonating || !requestUser.IsInRole("PRODUCER") || !requestUser.IsInRole("CONSUMER"))
                return RedirectToAction("Impersonate", "Account");

            var result = await _mediatr.Process(new CancelPurchaseOrderCommand(requestUser)
            {
                Id = id,
                SkipNotification = true,
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Process(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            if (!requestUser.IsImpersonating || !requestUser.IsInRole("PRODUCER"))
                return RedirectToAction("Impersonate", "Account");

            var result = await _mediatr.Process(new ProcessPurchaseOrderCommand(requestUser)
            {
                Id = id,
                SkipNotification = true,
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            if (!requestUser.IsImpersonating || !requestUser.IsInRole("PRODUCER"))
                return RedirectToAction("Impersonate", "Account");

            var result = await _mediatr.Process(new CompletePurchaseOrderCommand(requestUser)
            {
                Id = id,
                SkipNotification = true,
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deliver(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            if (!requestUser.IsImpersonating || !requestUser.IsInRole("PRODUCER"))
                return RedirectToAction("Impersonate", "Account");

            var result = await _mediatr.Process(new DeliverPurchaseOrderCommand(requestUser)
            {
                Id = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new RestorePurchaseOrderCommand(await GetRequestUser(token))
            {
                Id = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new DeletePurchaseOrderCommand(await GetRequestUser(token))
            {
                Id = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Index");
        }
    }
}
