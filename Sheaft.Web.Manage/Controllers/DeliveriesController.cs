using System;
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
using Sheaft.Mediatr.Delivery.Commands;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Controllers
{
    public class DeliveriesController : ManageController
    {
        public DeliveriesController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider)
            : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken token, DeliveryStatus? status = null, int page = 0,
            int take = 25)
        {
            if (page < 0)
                page = 0;

            if (take > 100)
                take = 100;

            var query = _context.Deliveries.AsNoTracking();

            var requestUser = await GetRequestUserAsync(token);
            if (requestUser.IsImpersonating())
            {
                if (requestUser.IsInRole(_roleOptions.Store.Value))
                    query = query.Where(p => p.ClientId == requestUser.Id);
                else
                    query = query.Where(p => p.ProducerId == requestUser.Id);
            }

            if (status != null)
                query = query.Where(q => q.Status == status);

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<ShortDeliveryViewModel>(_configurationProvider)
                .ToListAsync(token);

            ViewBag.Page = page;
            ViewBag.Take = take;
            ViewBag.Status = status;
            return View(entities);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var entity = await _context.Deliveries
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<DeliveryViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw SheaftException.NotFound();
            
            if(entity.DeliveryBatchId.HasValue)
                ViewBag.DeliveryBatch = await _context.DeliveryBatches
                    .AsNoTracking()
                    .Where(c => c.Id == entity.DeliveryBatchId.Value)
                    .ProjectTo<DeliveryBatchViewModel>(_configurationProvider)
                    .SingleOrDefaultAsync(token);

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DeliveryViewModel model, CancellationToken token)
        {
            var entity = await _context.Deliveries.SingleAsync(a => a.Id == model.Id, token);
            return RedirectToAction("Edit", new {entity.Id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new DeleteDeliveryCommand(await GetRequestUserAsync(token))
            {
                DeliveryId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Index");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateForm(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new GenerateDeliveryFormCommand(await GetRequestUserAsync(token))
            {
                DeliveryId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id});
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateReceipt(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new GenerateDeliveryReceiptCommand(await GetRequestUserAsync(token))
            {
                DeliveryId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id});
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new CompleteDeliveryCommand(await GetRequestUserAsync(token))
            {
                DeliveryId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id});
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Skip(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new SkipDeliveryCommand(await GetRequestUserAsync(token))
            {
                DeliveryId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id});
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Start(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new StartDeliveryCommand(await GetRequestUserAsync(token))
            {
                DeliveryId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id});
        }
    }
}