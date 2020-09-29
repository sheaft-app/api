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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Domain.Enums;

namespace Sheaft.Manage.Controllers
{
    public class JobsController : ManageController
    {
        private readonly ILogger<JobsController> _logger;

        public JobsController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider,
            ILogger<JobsController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken token, ProcessStatus? status = null, int page = 0, int take = 10)
        {
            if (page < 0)
                page = 0;

            if (take > 100)
                take = 100;

            var query = _context.Jobs.AsNoTracking();

            var requestUser = await GetRequestUser(token);
            if (requestUser.IsImpersonating)
                query = query.Where(p => p.User.Id == requestUser.Id);

            if (status != null)
                query = query.Where(j => j.Status == status);

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<JobViewModel>(_configurationProvider)
                .ToListAsync(token);

            var edited = (string)TempData["Edited"];
            ViewBag.Edited = !string.IsNullOrWhiteSpace(edited) ? JsonConvert.DeserializeObject(edited) : null;

            var restored = (string)TempData["Restored"];
            ViewBag.Restored = !string.IsNullOrWhiteSpace(restored) ? JsonConvert.DeserializeObject(restored) : null;

            ViewBag.Removed = TempData["Removed"];
            ViewBag.Page = page;
            ViewBag.Take = take;
            ViewBag.Status = status;

            return View(entities);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var entity = await _context.Jobs
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<JobViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(JobViewModel model, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Process(new UpdateJobCommand(requestUser)
            {
                Id = model.Id,
                Name = model.Name
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
        public async Task<IActionResult> Archive(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Process(new ArchiveJobCommand(requestUser)
            {
                Id = id
            }, token);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return RedirectToAction("Edit", new { id = id });
            }

            return RedirectToAction("Edit", new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pause(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Process(new PauseJobCommand(requestUser)
            {
                Id = id
            }, token);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return RedirectToAction("Edit", new { id = id });
            }

            return RedirectToAction("Edit", new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Resume(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Process(new ResumeJobCommand(requestUser)
            {
                Id = id
            }, token);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return RedirectToAction("Edit", new { id = id });
            }

            return RedirectToAction("Edit", new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Process(new CancelJobCommand(requestUser)
            {
                Id = id
            }, token);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return RedirectToAction("Edit", new { id = id });
            }

            return RedirectToAction("Edit", new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Retry(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Process(new RetryJobCommand(requestUser)
            {
                Id = id
            }, token);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return RedirectToAction("Edit", new { id = id });
            }

            return RedirectToAction("Edit", new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unarchive(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Process(new UnarchiveJobCommand(requestUser)
            {
                Id = id
            }, token);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return RedirectToAction("Edit", new { id = id });
            }

            return RedirectToAction("Edit", new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reset(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Process(new ResetJobCommand(requestUser)
            {
                Id = id
            }, token);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return RedirectToAction("Edit", new { id = id });
            }

            return RedirectToAction("Edit", new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token)
        {
            var entity = await _context.Jobs.SingleOrDefaultAsync(c => c.Id == id, token);
            var name = entity.Name;

            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Process(new DeleteJobCommand(requestUser)
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
            var entity = await _context.Jobs.SingleOrDefaultAsync(c => c.Id == id, token);
            var name = entity.Name;

            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Process(new RestoreJobCommand(requestUser)
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