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

namespace Sheaft.Web.Manage.Controllers
{
    public class LevelsController : ManageController
    {
        private readonly ILogger<LevelsController> _logger;

        public LevelsController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider,
            ILogger<LevelsController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
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

            var query = _context.Levels.AsNoTracking();

            var entities = await query
                .OrderBy(c => c.RequiredPoints)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<LevelViewModel>(_configurationProvider)
                .ToListAsync(token);

            ViewBag.Page = page;
            ViewBag.Take = take;

            return View(entities);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new LevelViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(LevelViewModel model, CancellationToken token)
        {
            var result = await _mediatr.Process(new CreateLevelCommand(await GetRequestUser(token))
            {
                Name = model.Name,
                RequiredPoints = model.RequiredPoints,
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
            var requestUser = await GetRequestUser(token);

            var entity = await _context.Levels
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<LevelViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LevelViewModel model, CancellationToken token)
        {
            var result = await _mediatr.Process(new UpdateLevelCommand(await GetRequestUser(token))
            {
                Id = model.Id,
                Name = model.Name,
                RequiredPoints = model.RequiredPoints
            }, token);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new DeleteLevelCommand(await GetRequestUser(token))
            {
                Id = id
            }, token);

            if (!result.Success)
                throw result.Exception;

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new RestoreLevelCommand(await GetRequestUser(token))
            {
                Id = id
            }, token);

            if (!result.Success)
                throw result.Exception;

            return RedirectToAction("Edit", new { id });
        }
    }
}
