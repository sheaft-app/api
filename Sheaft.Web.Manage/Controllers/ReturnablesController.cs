using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Commands;
using Sheaft.Exceptions;
using Sheaft.Application.Interop;
using Sheaft.Web.Manage.Models;
using Sheaft.Application.Models;
using Sheaft.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Web.Manage.Controllers
{
    public class ReturnablesController : ManageController
    {
        private readonly ILogger<ReturnablesController> _logger;

        public ReturnablesController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider,
            ILogger<ReturnablesController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
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

            var query = _context.Returnables.AsNoTracking();

            var requestUser = await GetRequestUser(token);
            if (requestUser.IsImpersonating)
            {
                query = query.Where(p => p.Producer.Id == requestUser.Id);
            }

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<ReturnableViewModel>(_configurationProvider)
                .ToListAsync(token);

            ViewBag.Page = page;
            ViewBag.Take = take;

            return View(entities);
        }

        [HttpGet]
        public async Task<IActionResult> Add(CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            if (!requestUser.IsImpersonating)
                return RedirectToAction("Impersonate", "Account");

            return View(new ReturnableViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ReturnableViewModel model, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            if (!requestUser.IsImpersonating)
            {
                ModelState.AddModelError("", "You must impersonate producer to create it.");
                return View(model);
            }

            var result = await _mediatr.Process(new CreateReturnableCommand(requestUser)
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

            return RedirectToAction("Edit", new { id = result.Data });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var entity = await _context.Returnables
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<ReturnableViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ReturnableViewModel model, CancellationToken token)
        {
            var result = await _mediatr.Process(new UpdateReturnableCommand(await GetRequestUser(token))
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

            return RedirectToAction("Edit", new { model.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new RestoreReturnableCommand(await GetRequestUser(token))
            {
                Id = id
            }, token);

            if (!result.Success)
                throw result.Exception;

            return RedirectToAction("Edit", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new DeleteReturnableCommand(await GetRequestUser(token))
            {
                Id = id
            }, token);

            if (!result.Success)
                throw result.Exception;

            return RedirectToAction("Index");
        }
    }
}
