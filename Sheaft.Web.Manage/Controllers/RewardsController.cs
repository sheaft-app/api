﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Commands;
using Sheaft.Exceptions;
using Sheaft.Application.Interop;
using Sheaft.Manage.Models;
using Sheaft.Application.Models;
using Sheaft.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Manage.Controllers
{
    public class RewardsController : ManageController
    {
        private readonly ILogger<RewardsController> _logger;

        public RewardsController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider,
            ILogger<RewardsController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
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

            var query = _context.Rewards.AsNoTracking();

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<RewardViewModel>(_configurationProvider)
                .ToListAsync(token);

            var edited = (string)TempData["Edited"];
            ViewBag.Edited = !string.IsNullOrWhiteSpace(edited) ? JsonConvert.DeserializeObject(edited) : null;

            ViewBag.Removed = TempData["Removed"];
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
            var requestUser = await GetRequestUser(token);

            var result = await _mediatr.Process(new CreateRewardCommand(requestUser)
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

            if (!result.Success)
            {
                ViewBag.Levels = await GetLevels(token);
                ViewBag.Departments = await GetDepartments(token);
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("Edit", new { id = result.Data });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);

            var entity = await _context.Rewards
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<RewardViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            ViewBag.Levels = await GetLevels(token);
            ViewBag.Departments = await GetDepartments(token);
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RewardViewModel model, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);

            var result = await _mediatr.Process(new UpdateRewardCommand(requestUser)
            {
                Id = model.Id,
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

            if (!result.Success)
            {
                ViewBag.Levels = await GetLevels(token);
                ViewBag.Departments = await GetDepartments(token);
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
            var entity = await _context.Rewards.SingleOrDefaultAsync(c => c.Id == id, token);
            var name = entity.Name;

            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Process(new DeleteRewardCommand(requestUser)
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
            var entity = await _context.Rewards.SingleOrDefaultAsync(c => c.Id == id, token);
            var name = entity.Name;

            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Process(new RestoreRewardCommand(requestUser)
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