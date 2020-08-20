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
using Sheaft.Exceptions;
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
    public class PackagingsController : ManageController
    {
        private readonly ILogger<PackagingsController> _logger;

        public PackagingsController(
            IAppDbContext context,
            IMapper mapper,
            IMediator mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider,
            ILogger<PackagingsController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
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

            var query = _context.Packagings.AsNoTracking();

            var requestUser = await GetRequestUser(token);
            if (requestUser.IsImpersonating)
            {
                query = query.Where(p => p.Producer.Id == requestUser.CompanyId);
            }

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<PackagingViewModel>(_configurationProvider)
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
        public async Task<IActionResult> Add(CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            if (!requestUser.IsImpersonating)
                throw new Exception("You must impersonate producer to create it.");

            return View(new PackagingViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(PackagingViewModel model, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            if (!requestUser.IsImpersonating)
            {
                ModelState.AddModelError("", "You must impersonate producer to create it.");
                return View(model);
            }

            var result = await _mediatr.Send(new CreatePackagingCommand(requestUser)
            {
                Description = model.Description,
                Name = model.Name,
                Vat = model.Vat,
                WholeSalePrice = model.WholeSalePrice
            }, token);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("Edit", new { id = result.Result });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            if (!requestUser.IsImpersonating)
                throw new Exception("You must impersonate product's producer to edit it.");

            var entity = await _context.Packagings
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<PackagingViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PackagingViewModel model, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            if (!requestUser.IsImpersonating)
            {
                ModelState.AddModelError("", "You must impersonate packaging's producer to edit it.");
                return View(model);
            }

            var result = await _mediatr.Send(new UpdatePackagingCommand(requestUser)
            {
                Id = model.Id,
                Description = model.Description,
                Name = model.Name,
                Vat = model.Vat,
                WholeSalePrice = model.WholeSalePrice
            }, token);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            TempData["Edited"] = JsonConvert.SerializeObject(new EntityViewModel { Id = model.Id, Name = model.Name });
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token)
        {
            var entity = await _context.Packagings.SingleOrDefaultAsync(c => c.Id == id, token);
            var name = entity.Name;

            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Send(new DeletePackagingCommand(requestUser)
            {
                Id = id
            }, token);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return RedirectToAction("Index");
            }

            TempData["Removed"] = name;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(Guid id, CancellationToken token)
        {
            var entity = await _context.Packagings.SingleOrDefaultAsync(c => c.Id == id, token);
            var name = entity.Name;

            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Send(new RestorePackagingCommand(requestUser)
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
    }
}
