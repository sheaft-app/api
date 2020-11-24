﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Exceptions;
using Sheaft.Application.Interop;
using Sheaft.Web.Manage.Models;
using Sheaft.Application.Models;
using Sheaft.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Web.Manage.Controllers
{
    public class ProducersController : ManageController
    {
        private readonly ILogger<ProducersController> _logger;

        public ProducersController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IConfigurationProvider configurationProvider,
            IOptionsSnapshot<RoleOptions> roleOptions,
            ILogger<ProducersController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
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

            var query = _context.Users.OfType<Producer>().AsNoTracking();

            var requestUser = await GetRequestUser(token);
            if (requestUser.IsImpersonating)
                query = query.Where(p => p.Id == requestUser.Id);

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<ProducerViewModel>(_configurationProvider)
                .ToListAsync(token);

            var edited = (string)TempData["Edited"];
            ViewBag.Edited = !string.IsNullOrWhiteSpace(edited) ? JsonConvert.DeserializeObject(edited) : null;

            var restored = (string)TempData["Restored"];
            ViewBag.Restored = !string.IsNullOrWhiteSpace(restored) ? JsonConvert.DeserializeObject(restored) : null;

            ViewBag.Removed = TempData["Removed"];
            ViewBag.Page = page;
            ViewBag.Take = take;

            return View(entities);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var entity = await _context.Users.OfType<Producer>()
                .Where(c => c.Id == id)
                .ProjectTo<ProducerViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            ViewBag.LegalsId = (await _context.FindSingleAsync<Legal>(c => c.User.Id == id, token))?.Id;
            ViewBag.BankAccountId = (await _context.FindSingleAsync<BankAccount>(c => c.User.Id == id, token))?.Id;

            ViewBag.Tags = await GetTags(token);
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProducerViewModel model, IFormFile picture, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            if (picture != null)
            {
                using (var ms = new MemoryStream())
                {
                    picture.CopyTo(ms);
                    model.Picture = Convert.ToBase64String(ms.ToArray());
                }
            }

            var entity = await _context.Users.OfType<Producer>().SingleOrDefaultAsync(c => c.Id == model.Id, token);
            var result = await _mediatr.Process(new UpdateProducerCommand(requestUser)
            {
                Id = model.Id,
                Address = _mapper.Map<FullAddressInput>(model.Address),
                OpenForNewBusiness = model.OpenForNewBusiness,
                Description = model.Description,
                Email = model.Email,
                Name = model.Name,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Kind = model.Kind,
                Phone = model.Phone,
                Tags = model.Tags,
                Picture = model.Picture,
                NotSubjectToVat = model.NotSubjectToVat
            }, token);

            if (!result.Success)
            {
                ViewBag.LegalsId = (await _context.FindSingleAsync<Legal>(c => c.User.Id == model.Id, token))?.Id;
                ViewBag.BankAccountId = (await _context.FindSingleAsync<BankAccount>(c => c.User.Id == model.Id, token))?.Id;
                ViewBag.Tags = await GetTags(token);

                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            TempData["Edited"] = JsonConvert.SerializeObject(new EntityViewModel { Id = model.Id, Name = model.Name });
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> CreateLegal(Guid userId, CancellationToken token)
        {
            var entity = await _context.Users.OfType<Business>()
                .AsNoTracking()
                .Where(c => c.Id == userId)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            ViewBag.Countries = await GetCountries(token);
            ViewBag.Nationalities = await GetNationalities(token);

            return View(new BusinessLegalViewModel
            {
                Owner = new OwnerViewModel { Id = userId },
                Name = entity.Name,
                Email = entity.Email
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLegal(BusinessLegalViewModel model, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            var user = await _context.FindByIdAsync<Producer>(model.Owner.Id, token);

            var result = await _mediatr.Process(new CreateBusinessLegalCommand(requestUser)
            {
                UserId = model.Owner.Id,
                Name = model.Name,
                Email = model.Email,
                Address = _mapper.Map<AddressInput>(model.Address),
                Kind = model.Kind,
                Owner = _mapper.Map<OwnerInput>(model.Owner),
                Siret = model.Siret,
                VatIdentifier = user != null && user.NotSubjectToVat ? null : model.VatIdentifier
            }, token);

            if (!result.Success)
            {
                ViewBag.Countries = await GetCountries(token);
                ViewBag.Nationalities = await GetNationalities(token);
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            TempData["Created"] = JsonConvert.SerializeObject(new EntityViewModel { Id = model.Owner.Id, Name = $"{model.Email}" });
            return RedirectToAction("Edit", new { id = model.Owner.Id });
        }

        [HttpGet]
        public async Task<IActionResult> CreateBankAccount(Guid userId, CancellationToken token)
        {
            var entity = await _context.Users.OfType<Business>()
                .AsNoTracking()
                .Where(c => c.Id == userId)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            ViewBag.Countries = await GetCountries(token);
            return View(new BankAccountViewModel
            {
                OwnerId = entity.Id,
                IsActive = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBankAccount(BankAccountViewModel model, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            var user = await _context.FindByIdAsync<Producer>(model.OwnerId, token);

            var result = await _mediatr.Process(new CreateBankAccountCommand(requestUser)
            {
                UserId = user.Id,
                Address = _mapper.Map<AddressInput>(model.Address),
                BIC = model.BIC,
                IBAN = model.IBAN,
                Name = model.Name,
                Owner = model.Owner
            }, token);

            if (!result.Success)
            {
                ViewBag.Countries = await GetCountries(token);
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            TempData["Created"] = JsonConvert.SerializeObject(new EntityViewModel { Id = model.OwnerId, Name = $"{model.Name}" });
            return RedirectToAction("Edit", new { id = model.OwnerId });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateLegal(Guid legalId, CancellationToken token)
        {
            var entity = await _context.Legals.OfType<BusinessLegal>()
                .Where(c => c.Id == legalId)
                .ProjectTo<BusinessLegalViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            ViewBag.Countries = await GetCountries(token);
            ViewBag.Nationalities = await GetNationalities(token);
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateLegal(BusinessLegalViewModel model, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            var user = await _context.FindByIdAsync<Producer>(model.Owner.Id, token);
            var result = await _mediatr.Process(new UpdateBusinessLegalCommand(requestUser)
            {
                Id = model.Id,
                Name = model.Name,
                Email = model.Email,
                Address = _mapper.Map<AddressInput>(model.Address),
                Kind = model.Kind,
                Owner = _mapper.Map<OwnerInput>(model.Owner),
                Siret = model.Siret,
                VatIdentifier = user != null && user.NotSubjectToVat ? null : model.VatIdentifier
            }, token);

            if (!result.Success)
            {
                ViewBag.Countries = await GetCountries(token);
                ViewBag.Nationalities = await GetNationalities(token);
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            TempData["Edited"] = JsonConvert.SerializeObject(new EntityViewModel { Id = model.Owner.Id, Name = $"{model.Email}" });
            return RedirectToAction("Edit", new { id = model.Owner.Id });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateBankAccount(Guid bankAccountId, CancellationToken token)
        {
            var entity = await _context.BankAccounts.Get(c => c.Id == bankAccountId)
                .ProjectTo<BankAccountViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            ViewBag.Countries = await GetCountries(token);
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBankAccount(BankAccountViewModel model, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Process(new UpdateBankAccountCommand(requestUser)
            {
                Id = model.Id,
                Address = _mapper.Map<AddressInput>(model.Address),
                BIC = model.BIC,
                IBAN = model.IBAN,
                Name = model.Name,
                Owner = model.Owner
            }, token);

            if (!result.Success)
            {
                ViewBag.Countries = await GetCountries(token);
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            TempData["Edited"] = JsonConvert.SerializeObject(new EntityViewModel { Id = model.OwnerId, Name = $"{model.Name}" });
            return RedirectToAction("Edit", new { id = model.OwnerId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token)
        {
            var entity = await _context.Users.OfType<Producer>().SingleOrDefaultAsync(c => c.Id == id, token);
            var name = entity.Name;

            var result = await _mediatr.Process(new RemoveUserCommand(await GetRequestUser(token))
            {
                Id = id
            }, token);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return RedirectToAction("Index");
            }

            TempData["Restored"] = JsonConvert.SerializeObject(new EntityViewModel { Id = entity.Id, Name = name });
            return RedirectToAction("Index");
        }

        private async Task<IEnumerable<CountryViewModel>> GetCountries(CancellationToken token)
        {
            return await _context.Countries
                .AsNoTracking()
                .ProjectTo<CountryViewModel>(_configurationProvider)
                .ToListAsync(token);
        }

        private async Task<IEnumerable<NationalityViewModel>> GetNationalities(CancellationToken token)
        {
            return await _context.Nationalities
                .AsNoTracking()
                .ProjectTo<NationalityViewModel>(_configurationProvider)
                .ToListAsync(token);
        }
    }
}
