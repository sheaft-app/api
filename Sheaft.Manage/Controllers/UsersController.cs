﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Commands;
using Sheaft.Infrastructure.Interop;
using Sheaft.Interop.Enums;
using Sheaft.Manage.Models;
using Sheaft.Models.ViewModels;
using Sheaft.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Manage.Controllers
{
    public class UsersController : ManageController
    {
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IAppDbContext context,
            IMapper mapper,
            IMediator mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider,
            ILogger<UsersController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken token, int page = 0, ProfileKind? kind = null, int take = 10)
        {
            if (page < 0)
                page = 0;

            if (take > 100)
                take = 100;

            var query = _context.Users.AsNoTracking();

            var requestUser = await GetRequestUser(token);
            if (requestUser.IsImpersonating)
            {
                query = query.Where(p => p.Id == requestUser.Id || (p.Company != null && p.Company.Id == requestUser.CompanyId));
            }

            if (kind.HasValue)
            {
                if(kind == ProfileKind.Consumer)
                    query = query.Where(p => p.Company == null);
                else
                    query = query.Where(p => p.Company.Kind == kind);
            }

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<UserViewModel>(_configurationProvider)
                .ToListAsync(token);

            var edited = (string)TempData["Edited"];
            ViewBag.Edited = !string.IsNullOrWhiteSpace(edited) ? JsonConvert.DeserializeObject(edited) : null;

            ViewBag.Removed = TempData["Removed"];
            ViewBag.Page = page;
            ViewBag.Take = take;
            ViewBag.Kind = kind;

            return View(entities);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var entity = await _context.Users
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<UserViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            ViewBag.Departments = await GetDepartments(token);
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel model, IFormFile picture, CancellationToken token)
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

            var result = await _mediatr.Send(new UpdateUserCommand(requestUser)
            {
                Id = model.Id,
                Anonymous = model.Anonymous,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phone = model.Phone,
                DepartmentId = model.DepartmentId,
                Picture = model.Picture
            }, token);

            if (!result.Success)
            {
                ViewBag.Departments = await GetDepartments(token);
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            TempData["Edited"] = JsonConvert.SerializeObject(new EditViewModel { Id = model.Id, Name = $"{model.FirstName} {model.LastName}" });
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token)
        {
            var entity = await _context.Users.SingleOrDefaultAsync(c => c.Id == id, token);
            var name = $"{entity.FirstName} {entity.LastName}";

            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Send(new DeleteUserCommand(requestUser)
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

        private async Task<IEnumerable<DepartmentViewModel>> GetDepartments(CancellationToken token)
        {
            return await _context.Departments.AsNoTracking().ProjectTo<DepartmentViewModel>(_configurationProvider).ToListAsync(token);
        }
    }
}
