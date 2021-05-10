using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core.Exceptions;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Mediatr.Reward.Commands;
using Sheaft.Options;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Controllers
{
    public class RewardsController : ManageController
    {
        public RewardsController(
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

            var query = _context.Rewards.AsNoTracking();

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<RewardViewModel>(_configurationProvider)
                .ToListAsync(token);

            ViewBag.Page = page;
            ViewBag.Take = take;

            return View(entities);
        }

        [HttpGet]
        public async Task<IActionResult> Add(CancellationToken token)
        {
            ViewBag.Levels = await GetLevels(token);
            ViewBag.Departments = await GetDepartments(token);

            return View(new RewardViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(RewardViewModel model, CancellationToken token)
        {
            var result = await _mediatr.Process(new CreateRewardCommand(await GetRequestUserAsync(token))
            {
                Name = model.Name,
                Contact = model.Contact,
                Description = model.Description,
                Url = model.Url,
                Email = model.Email,
                Picture = model.Picture,
                Phone = model.Phone,
                DepartmentId = model.DepartmentId,
                LevelId = model.LevelId
            }, token);

            if (!result.Succeeded)
            {
                ViewBag.Levels = await GetLevels(token);
                ViewBag.Departments = await GetDepartments(token);
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("Edit", new {id = result.Data});
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var entity = await _context.Rewards
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<RewardViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw SheaftException.NotFound();

            ViewBag.Levels = await GetLevels(token);
            ViewBag.Departments = await GetDepartments(token);
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RewardViewModel model, CancellationToken token)
        {
            var result = await _mediatr.Process(new UpdateRewardCommand(await GetRequestUserAsync(token))
            {
                RewardId = model.Id,
                Name = model.Name,
                Contact = model.Contact,
                Description = model.Description,
                Url = model.Url,
                Email = model.Email,
                Picture = model.Picture,
                Phone = model.Phone,
                DepartmentId = model.DepartmentId,
                LevelId = model.LevelId
            }, token);

            if (!result.Succeeded)
            {
                ViewBag.Levels = await GetLevels(token);
                ViewBag.Departments = await GetDepartments(token);
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("Edit", new {model.Id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new RestoreRewardCommand(await GetRequestUserAsync(token))
            {
                RewardId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new DeleteRewardCommand(await GetRequestUserAsync(token))
            {
                RewardId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Index");
        }

        private async Task<IEnumerable<DepartmentViewModel>> GetDepartments(CancellationToken token)
        {
            return await _context.Departments
                .AsNoTracking()
                .ProjectTo<DepartmentViewModel>(_configurationProvider)
                .ToListAsync(token);
        }

        private async Task<IEnumerable<LevelViewModel>> GetLevels(CancellationToken token)
        {
            return await _context.Levels
                .AsNoTracking()
                .ProjectTo<LevelViewModel>(_configurationProvider)
                .ToListAsync(token);
        }
    }
}