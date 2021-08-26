using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core.Exceptions;
using Sheaft.Core.Options;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr;
using Sheaft.Mediatr.Agreement.Commands;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Controllers
{
    public class AgreementsController : ManageController
    {
        public AgreementsController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider)
            : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken token, AgreementStatus? status = null, int page = 0,
            int take = 25)
        {
            if (page < 0)
                page = 0;

            if (take > 100)
                take = 100;

            var query = _context.Agreements.AsNoTracking();

            var requestUser = await GetRequestUserAsync(token);
            if (requestUser.IsImpersonating())
            {
                if (requestUser.IsInRole(_roleOptions.Store.Value))
                    query = query.Where(p => p.StoreId == requestUser.Id);
                else
                    query = query.Where(p => p.ProducerId == requestUser.Id);
            }

            if (status != null)
                query = query.Where(q => q.Status == status);

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<ShortAgreementViewModel>(_configurationProvider)
                .ToListAsync(token);

            ViewBag.Page = page;
            ViewBag.Take = take;
            ViewBag.Status = status;
            return View(entities);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var entity = await _context.Agreements
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<AgreementViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw SheaftException.NotFound();

            var deliveries = await _context.DeliveryModes
                .Where(d => d.ProducerId == entity.Producer.Id && (int) d.Kind > 4)
                .ToListAsync(token);

            var catalogs = await _context.Catalogs
                .Where(d => d.ProducerId == entity.Producer.Id && d.Kind == CatalogKind.Stores)
                .ToListAsync(token);

            ViewBag.Deliveries = deliveries.Select(d => new SelectListItem(d.Name, d.Id.ToString("D"))).ToList();
            ViewBag.Catalogs = catalogs.Select(d => new SelectListItem(d.Name, d.Id.ToString("D"))).ToList();

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AgreementViewModel model, CancellationToken token)
        {
            var entity = await _context.Agreements.SingleAsync(a => a.Id == model.Id, token);
            if (model.CatalogId.HasValue && entity.CatalogId != model.CatalogId)
            {
                var catalogResult =
                    await _mediatr.Process(
                        new ChangeAgreementCatalogCommand(new RequestUser() {Id = entity.ProducerId})
                            {AgreementId = entity.Id, CatalogId = model.CatalogId.Value}, token);

                if (!catalogResult.Succeeded)
                    throw catalogResult.Exception;
            }

            if (model.DeliveryModeId.HasValue && entity.DeliveryModeId != model.DeliveryModeId)
            {
                var deliveryResult =
                    await _mediatr.Process(
                        new ChangeAgreementDeliveryCommand(new RequestUser() {Id = entity.ProducerId})
                            {AgreementId = entity.Id, DeliveryId = model.DeliveryModeId.Value}, token);

                if (!deliveryResult.Succeeded)
                    throw deliveryResult.Exception;
            }

            if (model.Status != entity.Status
                && entity.Status != AgreementStatus.Cancelled
                && (model.Status == AgreementStatus.Accepted ||
                    model.Status == AgreementStatus.Refused))
            {
                Command command = null;
                switch (model.Status)
                {
                    case AgreementStatus.Accepted:
                        command = new AcceptAgreementCommand(new RequestUser()
                        {
                            Id = entity.CreatedByKind == ProfileKind.Producer ? entity.StoreId : entity.ProducerId
                        })
                        {
                            AgreementId = entity.Id,
                            CatalogId = model.CatalogId,
                            DeliveryId = model.DeliveryModeId
                        };
                        break;
                    case AgreementStatus.Refused:
                        command = new RefuseAgreementCommand(new RequestUser()
                        {
                            Id = entity.CreatedByKind == ProfileKind.Producer ? entity.StoreId : entity.ProducerId
                        })
                        {
                            AgreementId = entity.Id
                        };
                        break;
                }

                if (command != null)
                {
                    var result = await _mediatr.Process(command, token);
                    if (!result.Succeeded)
                        throw result.Exception;
                }
            }

            return RedirectToAction("Edit", new {entity.Id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reset(AgreementViewModel model, CancellationToken token)
        {
            var result = await _mediatr.Process(new ResetAgreementStatusToCommand(await GetRequestUserAsync(token))
            {
                AgreementId = model.Id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {model.Id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new DeleteAgreementCommand(await GetRequestUserAsync(token))
            {
                AgreementId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new RestoreAgreementCommand(await GetRequestUserAsync(token))
            {
                AgreementId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id});
        }
    }
}