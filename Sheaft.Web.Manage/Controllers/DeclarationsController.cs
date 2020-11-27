using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Commands;
using Sheaft.Exceptions;
using Sheaft.Application.Interop;
using Sheaft.Application.Models;
using Sheaft.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Sheaft.Domain.Models;
using Sheaft.Domain.Enums;

namespace Sheaft.Web.Manage.Controllers
{
    public class DeclarationsController : ManageController
    {
        private readonly ILogger<DeclarationsController> _logger;

        public DeclarationsController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider,
            ILogger<DeclarationsController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
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

            var legalIds = await _context.Legals
                .OfType<BusinessLegal>()
                .Get(c => c.DeclarationRequired && c.Declaration != null && c.Declaration.Status == DeclarationStatus.Validated)
                .Select(c => c.Id)
                .ToListAsync(token);

            var currentMonth = DateTimeOffset.UtcNow.Month;
            var query = await _context.PurchaseOrders.Get(d =>
                d.Status >= PurchaseOrderStatus.Accepted
                && d.Status < PurchaseOrderStatus.Refused)
                .Where(d => d.CreatedOn.Month == currentMonth && !legalIds.Contains(d.Vendor.Id))
                .Select(d => new { d.Vendor, d.TotalOnSalePrice })
                .ToListAsync(token);

            var results = query.GroupBy(c => c.Vendor).Select(d => new ProducerRequiredDeclarationViewModel
            {
                Id = d.Key.Id,
                Name = d.Key.Name,
                Email = d.Key.Email,
                Phone = d.Key.Phone,
                Total = d.Sum(e => e.TotalOnSalePrice)
            }).Where(d => d.Total > 100);

            var entities = results
                .OrderByDescending(c => c.Total)
                .Skip(page * take)
                .Take(take)
                .ToList();

            ViewBag.Page = page;
            ViewBag.Take = take;

            return View(entities);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var entity = await _context.Legals
                .OfType<BusinessLegal>()
                .Where(c => c.Declaration.Id == id)
                .ProjectTo<BusinessLegalViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            ViewBag.UserId = entity.Owner.Id;
            ViewBag.LegalId = entity.Id;

            return View(entity.Declaration);
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid legalId, CancellationToken token)
        {
            var entity = await _context.Legals
                .OfType<BusinessLegal>()
                .SingleOrDefaultAsync(c => c.Id == legalId, token);

            if (entity == null)
                throw new NotFoundException();

            var result = await _mediatr.Process(new CreateDeclarationCommand(await GetRequestUser(token))
            {
                LegalId = legalId
            }, token);

            if (!result.Success)
                throw result.Exception;

            return RedirectToAction("Edit", new { id = result.Data });
        }

        [HttpGet]
        public async Task<IActionResult> AddUbo(Guid declarationId, CancellationToken token)
        {
            ViewBag.Countries = await GetCountries(token);
            ViewBag.Nationalities = await GetNationalities(token);
            ViewBag.DeclarationId = declarationId;

            return View(new UboViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUbo(Guid declarationId, UboViewModel model, CancellationToken token)
        {
            var result = await _mediatr.Process(new CreateUboCommand(await GetRequestUser(token))
            {
                DeclarationId = declarationId,
                Address = _mapper.Map<AddressInput>(model.Address),
                BirthDate = model.BirthDate,
                BirthPlace = _mapper.Map<BirthAddressInput>(model.BirthPlace),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Nationality = model.Nationality

            }, token);

            if (!result.Success)
            {
                ViewBag.Countries = await GetCountries(token);
                ViewBag.Nationalities = await GetNationalities(token);
                ViewBag.DeclarationId = declarationId;
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("Edit", new { id = declarationId });
        }

        [HttpGet]
        public async Task<IActionResult> EditUbo(Guid declarationId, Guid uboId, CancellationToken token)
        {
            var ubo = await _context.Legals
                .OfType<BusinessLegal>()
                .SelectMany(c => c.Declaration.Ubos)
                .Where(c => c.Id == uboId)
                .ProjectTo<UboViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (ubo == null)
                throw new NotFoundException();

            ViewBag.Countries = await GetCountries(token);
            ViewBag.Nationalities = await GetNationalities(token);
            ViewBag.DeclarationId = declarationId;

            return View(ubo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUbo(Guid declarationId, UboViewModel model, CancellationToken token)
        {
            var result = await _mediatr.Process(new UpdateUboCommand(await GetRequestUser(token))
            {
                Id = model.Id,
                Address = _mapper.Map<AddressInput>(model.Address),
                BirthDate = model.BirthDate,
                BirthPlace = _mapper.Map<BirthAddressInput>(model.BirthPlace),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Nationality = model.Nationality
            }, token);

            if (!result.Success)
            {
                ViewBag.Countries = await GetCountries(token);
                ViewBag.Nationalities = await GetNationalities(token);
                ViewBag.DeclarationId = declarationId;
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("Edit", new { id = declarationId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Lock(Guid declarationId, CancellationToken token)
        {
            var result = await _mediatr.Process(new LockDeclarationCommand(await GetRequestUser(token))
            {
                DeclarationId = declarationId
            }, token);

            if (!result.Success)
                throw result.Exception;

            return RedirectToAction("Edit", new { id = declarationId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unlock(Guid declarationId, CancellationToken token)
        {
            var result = await _mediatr.Process(new UnLockDeclarationCommand(await GetRequestUser(token))
            {
                DeclarationId = declarationId
            }, token);

            if (!result.Success)
                throw result.Exception;

            return RedirectToAction("Edit", new { id = declarationId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Validate(Guid declarationId, CancellationToken token)
        {
            var result = await _mediatr.Process(new SubmitDeclarationCommand(await GetRequestUser(token))
            {
                DeclarationId = declarationId
            }, token);

            if (!result.Success)
                throw result.Exception;

            return RedirectToAction("Edit", new { id = declarationId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUbo(Guid declarationId, Guid uboId, CancellationToken token)
        {
            var result = await _mediatr.Process(new DeleteUboCommand(await GetRequestUser(token))
            {
                Id = uboId
            }, token);

            if (!result.Success)
                throw result.Exception;

            return RedirectToAction("Edit", new { id = declarationId });
        }
    }
}
