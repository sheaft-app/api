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
using Sheaft.Interop;
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
    public class ProductsController : ManageController
    {
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(
            IAppDbContext context,
            IMapper mapper,
            IMediator mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider,
            ILogger<ProductsController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
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

            var query = _context.Products.AsNoTracking();

            var requestUser = await GetRequestUser(token);
            if (requestUser.IsImpersonating)
            {
                query = query.Where(p => p.Producer.Id == requestUser.CompanyId);
            }

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<ProductViewModel>(_configurationProvider)
                .ToListAsync(token);

            var edited = (string)TempData["Edited"];
            ViewBag.Edited = !string.IsNullOrWhiteSpace(edited) ? JsonConvert.DeserializeObject(edited) : null;

            ViewBag.Removed = TempData["Removed"];
            ViewBag.Page = page;
            ViewBag.Take = take;

            return View(entities);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            if (!requestUser.IsImpersonating)
                throw new Exception("You must impersonate product's producer to edit it.");

            var entity = await _context.Products
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<ProductViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            ViewBag.Tags = await GetTags(token);
            ViewBag.Packagings = await GetPackagings(requestUser, token);

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel model, IFormFile picture, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            if(!requestUser.IsImpersonating)
            {
                ViewBag.Tags = await GetTags(token);
                ViewBag.Packagings = await GetPackagings(requestUser, token);

                ModelState.AddModelError("", "You must impersonate product's producer to edit it.");
                return View(model);
            }

            if (picture != null)
            {
                using (var ms = new MemoryStream())
                {
                    picture.CopyTo(ms);
                    model.Picture = Convert.ToBase64String(ms.ToArray());
                }
            }

            var result = await _mediatr.Send(new UpdateProductCommand(requestUser)
            {
                Id = model.Id,
                Description = model.Description,
                Name = model.Name,
                Available = model.Available,
                PackagingId = model.PackagingId,
                Picture = model.Picture,
                QuantityPerUnit = model.QuantityPerUnit,
                Reference = model.Reference,
                Tags = model.Tags,
                Unit = model.Unit,
                Vat = model.Vat,
                Weight = model.Weight,
                WholeSalePricePerUnit = model.WholeSalePricePerUnit
            }, token);

            if (!result.Success)
            {
                ViewBag.Tags = await GetTags(token);
                ViewBag.Packagings = await GetPackagings(requestUser, token);

                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            TempData["Edited"] = JsonConvert.SerializeObject(new EditViewModel { Id = model.Id, Name = model.Name });
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token)
        {
            var entity = await _context.Products.SingleOrDefaultAsync(c => c.Id == id, token);

            _context.Remove(entity);
            await _context.SaveChangesAsync(token);

            TempData["Removed"] = entity.Name;
            return RedirectToAction("Index");
        }

        private async Task<List<PackagingViewModel>> GetPackagings(IRequestUser requestUser, CancellationToken token)
        {
            return await _context.Packagings
                            .Where(c => c.Producer.Id == requestUser.CompanyId && !c.RemovedOn.HasValue)
                            .ProjectTo<PackagingViewModel>(_configurationProvider)
                            .ToListAsync(token);
        }
    }
}
