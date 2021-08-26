using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core.Exceptions;
using Sheaft.Core.Options;
using Sheaft.Mediatr.Setting.Commands;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Controllers
{
    public class SettingsController : ManageController
    {
        public SettingsController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider)
            : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken token, int page = 0, int take = 50)
        {
            if (page < 0)
                page = 0;

            if (take > 100)
                take = 100;

            var query = _context.Settings.AsNoTracking();

            var entities = await query
                .OrderByDescending(c => c.Id)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<SettingViewModel>(_configurationProvider)
                .ToListAsync(token);

            ViewBag.Page = page;
            ViewBag.Take = take;

            return View(entities);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new SettingViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(SettingViewModel model, CancellationToken token)
        {
            var result = await _mediatr.Process(_mapper.Map(model, new CreateSettingCommand(await GetRequestUserAsync(token))), token);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var entity = await _context.Settings
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<SettingViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw SheaftException.NotFound();

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SettingViewModel model, CancellationToken token)
        {
            var result = await _mediatr.Process(_mapper.Map(model, new UpdateSettingCommand(await GetRequestUserAsync(token))), token);
            if (!result.Succeeded)
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
            var result = await _mediatr.Process(new DeleteSettingCommand(await GetRequestUserAsync(token)){SettingId = id}, token);
            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Index");
        }
    }
}