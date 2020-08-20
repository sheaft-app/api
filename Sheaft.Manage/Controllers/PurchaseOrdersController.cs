using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Commands;
using Sheaft.Core.Extensions;
using Sheaft.Infrastructure.Interop;
using Sheaft.Manage.Models;
using Sheaft.Models.ViewModels;
using Sheaft.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Manage.Controllers
{
    public class PurchaseOrdersController : ManageController
    {
        private readonly ILogger<PurchaseOrdersController> _logger;

        public PurchaseOrdersController(
            IAppDbContext context,
            IMapper mapper,
            IMediator mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider,
            ILogger<PurchaseOrdersController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken token, int page = 0, int take = 10)
        {
            if (page < 0)
                page = 0;

            if (take > 100)
                take = 100;

            var query = _context.PurchaseOrders.AsNoTracking();

            var requestUser = await GetRequestUser(token);
            if (requestUser.IsImpersonating)
            {
                if(requestUser.IsInRole(_roleOptions.Producer.Value))
                    query = query.Where(p => p.Vendor.Id == requestUser.CompanyId);
                else if (requestUser.IsInRole(_roleOptions.Store.Value))
                    query = query.Where(p => p.Sender.Id == requestUser.CompanyId);
                else
                    query = query.Where(p => p.Sender.Id == requestUser.Id);
            }

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<PurchaseOrderViewModel>(_configurationProvider)
                .ToListAsync(token);

            var edited = (string)TempData["Edited"];
            ViewBag.Edited = !string.IsNullOrWhiteSpace(edited) ? JsonConvert.DeserializeObject(edited) : null;

            ViewBag.Removed = TempData["Removed"];
            ViewBag.Page = page;
            ViewBag.Take = take;

            return View(entities);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            if (!requestUser.IsImpersonating)
                throw new Exception("You must impersonate purchaseOrder's vendor or sender to edit it.");

            var entity = await _context.PurchaseOrders
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<PurchaseOrderViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PackagingViewModel model, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            if (!requestUser.IsImpersonating)
            {
                ModelState.AddModelError("", "You must impersonate purchaseOrder's vendor or sender to edit it.");
                return View(model);
            }

            //var result = await _mediatr.Send(new UpdatePackagingCommand(requestUser)
            //{
            //    Id = model.Id,
            //    Description = model.Description,
            //    Name = model.Name,
            //    Vat = model.Vat,
            //}, token);

            //if (!result.Success)
            //{
            //    ModelState.AddModelError("", result.Exception.Message);
            //    return View(model);
            //}

            TempData["Edited"] = JsonConvert.SerializeObject(new EditViewModel { Id = model.Id, Name = model.Name });
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token)
        {
            var entity = await _context.PurchaseOrders.SingleOrDefaultAsync(c => c.Id == id, token);
            var reference = entity.Reference;

            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Send(new DeletePurchaseOrderCommand(requestUser)
            {
                Id = id
            }, token);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return RedirectToAction("Index");
            }

            TempData["Removed"] = reference;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(Guid id, CancellationToken token)
        {
            var entity = await _context.PurchaseOrders.SingleOrDefaultAsync(c => c.Id == id, token);
            var name = entity.Reference;

            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Send(new RestorePurchaseOrderCommand(requestUser)
            {
                Id = id
            }, token);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return RedirectToAction("Index");
            }

            TempData["Restored"] = name;
            return RedirectToAction("Index");
        }
    }
}
