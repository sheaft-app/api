using AutoMapper;
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
    public class ConsumersController : ManageController
    {
        private readonly ILogger<ConsumersController> _logger;

        public ConsumersController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider,
            ILogger<ConsumersController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
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

            var query = _context.Users.OfType<Consumer>().AsNoTracking();

            var requestUser = await GetRequestUser(token);
            if (requestUser.IsImpersonating)
                query = query.Where(p => p.Id == requestUser.Id);

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<ConsumerViewModel>(_configurationProvider)
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
            var entity = await _context.Users.OfType<Consumer>()
                .Where(c => c.Id == id)
                .ProjectTo<ConsumerViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ConsumerViewModel model, IFormFile picture, CancellationToken token)
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

            var result = await _mediatr.Process(new UpdateConsumerCommand(requestUser)
            {
                Id = model.Id,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phone = model.Phone,
                Picture = model.Picture
            }, token);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            TempData["Edited"] = JsonConvert.SerializeObject(new EntityViewModel { Id = model.Id, Name = $"{model.FirstName} {model.LastName}" });
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> CreateLegal(Guid userId, CancellationToken token)
        {
            var entity = await _context.Users.OfType<Consumer>()
                .AsNoTracking()
                .Where(c => c.Id == userId)
                .ProjectTo<ConsumerViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            ViewBag.Countries = await GetCountries(token);
            ViewBag.Nationalities = await GetNationalities(token);

            return View(new ConsumerLegalViewModel
            {
                Owner = new OwnerViewModel { Id = userId }
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLegal(ConsumerLegalViewModel model, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Process(new CreateConsumerLegalCommand(requestUser)
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

            TempData["Created"] = JsonConvert.SerializeObject(new EntityViewModel { Id = model.Owner.Id, Name = $"{model.Owner.FirstName} {model.Owner.LastName}" });
            return RedirectToAction("Edit", new { id = model.Owner.Id });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateLegal(Guid userId, CancellationToken token)
        {
            var entity = await _context.Legals.OfType<ConsumerLegal>()
                .Where(c => c.User.Id == userId)
                .ProjectTo<ConsumerLegalViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                return RedirectToAction("CreateLegal", new { userId = userId });

            ViewBag.Countries = await GetCountries(token);
            ViewBag.Nationalities = await GetNationalities(token);
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateLegal(ConsumerLegalViewModel model, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Process(new UpdateConsumerLegalCommand(requestUser)
            {
                Id = model.Id,
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

            TempData["Edited"] = JsonConvert.SerializeObject(new EntityViewModel { Id = model.Owner.Id, Name = $"{model.Owner.FirstName} {model.Owner.LastName}" });
            return RedirectToAction("Edit", new { id = model.Owner.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token)
        {
            var entity = await _context.Users.OfType<Consumer>().SingleOrDefaultAsync(c => c.Id == id, token);
            var name = entity.Name;

            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Process(new RemoveUserCommand(requestUser)
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
