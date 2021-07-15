using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core.Exceptions;
using Sheaft.Mediatr.Batch.Commands;
using Sheaft.Options;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Controllers
{
    public class BatchesController : ManageController
    {
        public BatchesController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider)
            : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken token, int page = 0,
            int take = 25)
        {
            if (page < 0)
                page = 0;

            if (take > 100)
                take = 100;

            var query = _context.Batches.AsNoTracking();

            var requestUser = await GetRequestUserAsync(token);
            if (requestUser.IsImpersonating())
            {
                query = query.Where(p => p.ProducerId == requestUser.Id);
            }

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<ShortBatchViewModel>(_configurationProvider)
                .ToListAsync(token);

            ViewBag.Page = page;
            ViewBag.Take = take;
            return View(entities);
        }

        [HttpGet]
        public async Task<IActionResult> Add(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUserAsync(token);
            if (!requestUser.IsImpersonating())
                return RedirectToAction("Impersonate", "Account");

            return View(new BatchViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(BatchViewModel model, CancellationToken token)
        {
            var requestUser = await GetRequestUserAsync(token);
            if (!requestUser.IsImpersonating())
            {
                ModelState.AddModelError("", "You must impersonate producer to create it.");
                return View(model);
            }

            var result = await _mediatr.Process(new CreateBatchCommand(requestUser)
            {
                Comment = model.Comment, Number = model.Number, DLC = model.DLC, DLUO = model.DLUO
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {Id = result.Data});
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUserAsync(token);
            if (!requestUser.IsImpersonating())
                return RedirectToAction("Impersonate", "Account");

            var entity = await _context.Batches
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<BatchViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw SheaftException.NotFound();

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BatchViewModel model, CancellationToken token)
        {
            var requestUser = await GetRequestUserAsync(token);
            if (!requestUser.IsImpersonating())
            {
                ModelState.AddModelError("", "You must impersonate producer to create it.");
                return View(model);
            }

            var result = await _mediatr.Process(new UpdateBatchCommand(requestUser)
            {
                BatchId = model.Id, Comment = model.Comment, Number = model.Number, DLC = model.DLC, DLUO = model.DLUO
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {model.Id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new DeleteBatchCommand(await GetRequestUserAsync(token))
            {
                BatchId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new RestoreBatchCommand(await GetRequestUserAsync(token))
            {
                BatchId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Index");
        }
    }
}