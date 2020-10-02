using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Commands;
using Sheaft.Core;
using Sheaft.Domain.Models;
using Sheaft.Exceptions;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Manage.Models;
using Sheaft.Application.Models;
using Sheaft.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Manage.Controllers
{
    public class StoresController : ManageController
    {
        private readonly ILogger<StoresController> _logger;

        public StoresController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IConfigurationProvider configurationProvider,
            IOptionsSnapshot<RoleOptions> roleOptions,
            ILogger<StoresController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
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

            var query = _context.Users.OfType<Store>().AsNoTracking();

            var requestUser = await GetRequestUser(token);
            if (requestUser.IsImpersonating)
                query = query.Where(p => p.Id == requestUser.Id);

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<StoreViewModel>(_configurationProvider)
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
            var entity = await _context.Users.OfType<Store>()
                .Where(c => c.Id == id)
                .ProjectTo<StoreViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            ViewBag.Tags = await GetTags(token);
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StoreViewModel model, IFormFile picture, CancellationToken token)
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

            var entity = await _context.Users.OfType<Store>().SingleOrDefaultAsync(c => c.Id == model.Id, token);
            
            var store = await _context.Users.OfType<Store>().SingleOrDefaultAsync(c => c.Id == model.Id, token);
            var result = await _mediatr.Process(new UpdateStoreCommand(requestUser)
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
                OpeningHours = store.OpeningHours?.GroupBy(oh => new { oh.From, oh.To }).Select(c => new TimeSlotGroupInput { From = c.Key.From, To = c.Key.To, Days = c.Select(o => o.Day) })
            }, token);

            if (!result.Success)
            {
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
            var entity = await _context.Users.OfType<Store>()
                .AsNoTracking()
                .Where(c => c.Id == userId)
                .ProjectTo<StoreViewModel>(_configurationProvider)
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
            var result = await _mediatr.Process(new CreateBusinessLegalCommand(requestUser)
            {
                UserId = model.Owner.Id,
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

            TempData["Created"] = JsonConvert.SerializeObject(new EntityViewModel { Id = model.Owner.Id, Name = $"{model.Email}" });
            return RedirectToAction("Edit", new { id = model.Owner.Id });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateLegal(Guid userId, CancellationToken token)
        {
            var entity = await _context.Legals.OfType<BusinessLegal>()
                .Where(c => c.User.Id == userId)
                .ProjectTo<BusinessLegalViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                return RedirectToAction("CreateLegal", new { userId = userId });

            ViewBag.Countries = await GetCountries(token);
            ViewBag.Nationalities = await GetNationalities(token);
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateLegal(BusinessLegalViewModel model, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Process(new UpdateBusinessLegalCommand(requestUser)
            {
                Id = model.Id,
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

            TempData["Edited"] = JsonConvert.SerializeObject(new EntityViewModel { Id = model.Owner.Id, Name = $"{model.Email}" });
            return RedirectToAction("Edit", new { id = model.Owner.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token)
        {
            var entity = await _context.Users.OfType<Store>().SingleOrDefaultAsync(c => c.Id == id, token);
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
            return await _context.Countries.AsNoTracking().ProjectTo<CountryViewModel>(_configurationProvider).ToListAsync(token);
        }

        private async Task<IEnumerable<NationalityViewModel>> GetNationalities(CancellationToken token)
        {
            return await _context.Nationalities.AsNoTracking().ProjectTo<NationalityViewModel>(_configurationProvider).ToListAsync(token);
        }
    }
}
