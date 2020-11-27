﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Exceptions;
using Sheaft.Application.Interop;
using Sheaft.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Domain.Enums;
using Sheaft.Application.Models;

namespace Sheaft.Web.Manage.Controllers
{
    public class PayinsController : ManageController
    {
        private readonly ILogger<PayinsController> _logger;
        private RoutineOptions _routineOptions;

        public PayinsController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider,
            ILogger<PayinsController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
            _logger = logger;
            _routineOptions = routineOptions.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken token, TransactionStatus? status = null, int page = 0, int take = 10)
        {
            if (page < 0)
                page = 0;

            if (take > 100)
                take = 100;

            var query = _context.Payins.AsNoTracking();

            var requestUser = await GetRequestUser(token);
            if (requestUser.IsImpersonating)
                query = query.Where(p => p.Author.Id == requestUser.Id);

            if (status != null)
                query = query.Where(t => t.Status == status);

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<PayinViewModel>(_configurationProvider)
                .ToListAsync(token);

            ViewBag.Page = page;
            ViewBag.Take = take;
            ViewBag.Status = status;

            return View(entities);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var entity = await _context.Payins
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<PayinViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            return View(entity);
        }
    }
}
