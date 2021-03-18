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
using Sheaft.Application.Models;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Options;
using Sheaft.Services.Declaration.Commands;
using Sheaft.Services.Ubo.Commands;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Controllers
{
    public class DeclarationsController : ManageController
    {
        public DeclarationsController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider)
            : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
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
                throw SheaftException.NotFound();

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
                throw SheaftException.NotFound();

            var result = await _mediatr.Process(new CreateDeclarationCommand(await GetRequestUser(token))
            {
                LegalId = legalId
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id = result.Data});
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
                Address = _mapper.Map<AddressDto>(model.Address),
                BirthDate = model.BirthDate,
                BirthPlace = _mapper.Map<BirthAddressDto>(model.BirthPlace),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Nationality = model.Nationality
            }, token);

            if (!result.Succeeded)
            {
                ViewBag.Countries = await GetCountries(token);
                ViewBag.Nationalities = await GetNationalities(token);
                ViewBag.DeclarationId = declarationId;
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("Edit", new {id = declarationId});
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
                throw SheaftException.NotFound();

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
                UboId = model.Id,
                Address = _mapper.Map<AddressDto>(model.Address),
                BirthDate = model.BirthDate,
                BirthPlace = _mapper.Map<BirthAddressDto>(model.BirthPlace),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Nationality = model.Nationality
            }, token);

            if (!result.Succeeded)
            {
                ViewBag.Countries = await GetCountries(token);
                ViewBag.Nationalities = await GetNationalities(token);
                ViewBag.DeclarationId = declarationId;
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("Edit", new {id = declarationId});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Lock(Guid declarationId, CancellationToken token)
        {
            var result = await _mediatr.Process(new LockDeclarationCommand(await GetRequestUser(token))
            {
                DeclarationId = declarationId
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id = declarationId});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unlock(Guid declarationId, CancellationToken token)
        {
            var result = await _mediatr.Process(new UnLockDeclarationCommand(await GetRequestUser(token))
            {
                DeclarationId = declarationId
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id = declarationId});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Validate(Guid declarationId, CancellationToken token)
        {
            var result = await _mediatr.Process(new SubmitDeclarationCommand(await GetRequestUser(token))
            {
                DeclarationId = declarationId
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id = declarationId});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUbo(Guid declarationId, Guid uboId, CancellationToken token)
        {
            var result = await _mediatr.Process(new DeleteUboCommand(await GetRequestUser(token))
            {
                UboId = uboId
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id = declarationId});
        }
    }
}