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
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Mediatr.DeliveryMode.Commands;
using Sheaft.Options;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Controllers
{
    public class DeliveryModesController : ManageController
    {
        public DeliveryModesController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider)
            : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken token, int page = 0, int take = 50)
        {
            if (page < 0)
                page = 0;

            if (take > 100)
                take = 100;

            var query = _context.DeliveryModes.AsNoTracking();

            var requestUser = await GetRequestUserAsync(token);
            if (requestUser.IsImpersonating())
            {
                query = query.Where(p => p.ProducerId == requestUser.Id);
            }

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<ShortDeliveryModeViewModel>(_configurationProvider)
                .ToListAsync(token);

            ViewBag.Page = page;
            ViewBag.Take = take;

            return View(entities);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var entity = await _context.DeliveryModes
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<DeliveryModeViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw SheaftException.NotFound();

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DeliveryModeViewModel model, CancellationToken token)
        {
            var entity = await _context.DeliveryModes
                .Where(c => c.Id == model.Id)
                .SingleOrDefaultAsync(token);
            
            var result = await _mediatr.Process(new UpdateDeliveryModeCommand(new RequestUser(){Id = entity.ProducerId})
            {
                DeliveryModeId = model.Id,
                Description = model.Description,
                Name = model.Name,
                Address = _mapper.Map<AddressDto>(model.Address),
                Kind = model.Kind,
                Available = model.Available,
                MaxPurchaseOrdersPerTimeSlot = model.MaxPurchaseOrdersPerTimeSlot,
                AutoAcceptRelatedPurchaseOrder = model.AutoAcceptRelatedPurchaseOrder,
                AutoCompleteRelatedPurchaseOrder = model.AutoCompleteRelatedPurchaseOrder,
                LockOrderHoursBeforeDelivery = model.LockOrderHoursBeforeDelivery,
                Closings = model.Closings?
                    .Where(d => !d.Remove && d.ClosedFrom.HasValue && d.ClosedTo.HasValue)
                    .Select(c => new ClosingInputDto
                    {
                        Id = c.Id,
                        From = c.ClosedFrom.Value,
                        To = c.ClosedTo.Value,
                        Reason = c.Reason
                    }),
                Agreements = model.Agreements?
                    .Select(p => new AgreementPositionDto
                    {
                        Id = p.Id,
                        Position = p.Position
                    }),
                DeliveryHours = model.DeliveryHours?
                    .Where(d => !d.Remove && d.Day.HasValue && d.From.HasValue && d.To.HasValue)
                    .GroupBy(d => new {d.From, d.To})
                    .Select(d => new TimeSlotGroupDto
                    {
                        Days = d.Select(e => e.Day.Value).ToList(),
                        From = d.Key.From.Value,
                        To = d.Key.To.Value
                    }),
            }, token);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("Edit", new {model.Id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDeliveryHour(Guid id, TimeSlotViewModel deliveryHour, CancellationToken token)
        {
            return RedirectToAction("Edit", new {id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddClosing(Guid id, ClosingViewModel closing, CancellationToken token)
        {
            return RedirectToAction("Edit", new {id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new DeleteDeliveryModeCommand(await GetRequestUserAsync(token))
            {
                DeliveryModeId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new RestoreDeliveryModeCommand(await GetRequestUserAsync(token))
            {
                DeliveryModeId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id});
        }
    }
}