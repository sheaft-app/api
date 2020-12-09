using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Exceptions;
using Sheaft.Application.Interop;
using Sheaft.Application.Models;
using Sheaft.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Domain.Enums;

namespace Sheaft.Web.Manage.Controllers
{
    public class LegalsController : ManageController
    {
        private readonly ILogger<LegalsController> _logger;

        public LegalsController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider,
            ILogger<LegalsController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken token, LegalKind? kind = null, int page = 0, int take = 50)
        {
            if (page < 0)
                page = 0;

            if (take > 100)
                take = 100;

            var query = _context.Legals.AsNoTracking();
            if (kind != null)
                query = query.Where(p => p.Kind == kind);

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<LegalViewModel>(_configurationProvider)
                .ToListAsync(token);

            ViewBag.Page = page;
            ViewBag.Take = take;
            ViewBag.Kind = kind;

            return View(entities);
        }

        [HttpGet]
        public async Task<IActionResult> EditUserLegals(Guid userId, CancellationToken token)
        {
            var entity = await _context.Legals
                .AsNoTracking()
                .Where(c => c.User.Id == userId)
                .SingleOrDefaultAsync(token);

            if (entity.Kind != LegalKind.Natural)
                return RedirectToAction("EditLegalBusiness", new { entity.Id });

            return RedirectToAction("EditLegalConsumer", new { entity.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var entity = await _context.Legals
                .AsNoTracking()
                .Where(c => c.Id == id)
                .SingleOrDefaultAsync(token);

            if (entity.Kind != LegalKind.Natural)
                return RedirectToAction("EditLegalBusiness", new { entity.Id });

            return RedirectToAction("EditLegalConsumer", new { entity.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid userId, CancellationToken token)
        {
            var entity = await _context.Users
                .AsNoTracking()
                .Where(c => c.Id == userId)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            if (entity.Kind != ProfileKind.Consumer)
                return RedirectToAction("CreateLegalBusiness", new { userId });

            return RedirectToAction("CreateLegalConsumer", new { userId });
        }

        [HttpGet]
        public async Task<IActionResult> CreateLegalBusiness(Guid userId, CancellationToken token)
        {
            var entity = await _context.Users
                .AsNoTracking()
                .Where(c => c.Id == userId)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            ViewBag.Countries = await GetCountries(token);
            ViewBag.Nationalities = await GetNationalities(token);

            return View(new BusinessLegalViewModel
            {
                Owner = new OwnerViewModel { Id = userId }
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLegalBusiness(BusinessLegalViewModel model, CancellationToken token)
        {
            var result = await _mediatr.Process(new CreateBusinessLegalCommand(await GetRequestUser(token))
            {
                UserId = model.Owner.Id,
                Name = model.Name,
                Email = model.Email,
                Address = _mapper.Map<AddressInput>(model.Address),
                Kind = model.Kind,
                Owner = _mapper.Map<OwnerInput>(model.Owner),
                Siret = model.Siret,
                VatIdentifier = model.VatIdentifier
            }, token);

            if (!result.Success)
            {
                ViewBag.Countries = await GetCountries(token);
                ViewBag.Nationalities = await GetNationalities(token);
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("EditLegalBusiness", new { id = result.Data });
        }

        [HttpGet]
        public async Task<IActionResult> CreateLegalConsumer(Guid userId, CancellationToken token)
        {
            var entity = await _context.Users
                .AsNoTracking()
                .Where(c => c.Id == userId)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            ViewBag.Countries = await GetCountries(token);
            ViewBag.Nationalities = await GetNationalities(token);

            return View(new ConsumerLegalViewModel
            {
                Kind = LegalKind.Natural,
                Owner = new OwnerViewModel { Id = userId }
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLegalConsumer(ConsumerLegalViewModel model, CancellationToken token)
        {
            var result = await _mediatr.Process(new CreateConsumerLegalCommand(await GetRequestUser(token))
            {
                UserId = model.Owner.Id,
                Email = model.Owner.Email,
                FirstName = model.Owner.FirstName,
                LastName = model.Owner.LastName,
                Address = _mapper.Map<AddressInput>(model.Owner.Address),
                BirthDate = model.Owner.BirthDate,
                CountryOfResidence = model.Owner.CountryOfResidence,
                Nationality = model.Owner.Nationality
            }, token);

            if (!result.Success)
            {
                ViewBag.Countries = await GetCountries(token);
                ViewBag.Nationalities = await GetNationalities(token);
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("EditLegalConsumer", new { id = result.Data });
        }

        [HttpGet]
        public async Task<IActionResult> EditLegalBusiness(Guid id, CancellationToken token)
        {
            var entity = await _context.Legals.OfType<BusinessLegal>()
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<BusinessLegalViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            ViewBag.Countries = await GetCountries(token);
            ViewBag.Nationalities = await GetNationalities(token);
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLegalBusiness(BusinessLegalViewModel model, CancellationToken token)
        {
            var result = await _mediatr.Process(new UpdateBusinessLegalCommand(await GetRequestUser(token))
            {
                Id = model.Id,
                Name = model.Name,
                Email = model.Email,
                Address = _mapper.Map<AddressInput>(model.Address),
                Kind = model.Kind,
                Validation = model.Validation,
                Owner = _mapper.Map<OwnerInput>(model.Owner),
                Siret = model.Siret,
                VatIdentifier = model.VatIdentifier
            }, token);

            if (!result.Success)
            {
                ViewBag.Countries = await GetCountries(token);
                ViewBag.Nationalities = await GetNationalities(token);
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("EditLegalBusiness", new { model.Id });
        }

        [HttpGet]
        public async Task<IActionResult> EditLegalConsumer(Guid id, CancellationToken token)
        {
            var entity = await _context.Legals.OfType<ConsumerLegal>()
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<ConsumerLegalViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            ViewBag.Countries = await GetCountries(token);
            ViewBag.Nationalities = await GetNationalities(token);
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLegalConsumer(ConsumerLegalViewModel model, CancellationToken token)
        {
            var result = await _mediatr.Process(new UpdateConsumerLegalCommand(await GetRequestUser(token))
            {
                Id = model.Id,
                Email = model.Owner.Email,
                FirstName = model.Owner.FirstName,
                LastName = model.Owner.LastName,
                Address = _mapper.Map<AddressInput>(model.Owner.Address),
                BirthDate = model.Owner.BirthDate,
                Validation = model.Validation,
                CountryOfResidence = model.Owner.CountryOfResidence,
                Nationality = model.Owner.Nationality
            }, token);

            if (!result.Success)
            {
                ViewBag.Countries = await GetCountries(token);
                ViewBag.Nationalities = await GetNationalities(token);
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("EditLegalConsumer", new { model.Id });
        }
    }
}
