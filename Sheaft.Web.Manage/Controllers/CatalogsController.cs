using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core.Exceptions;
using Sheaft.Mediatr.Catalog.Commands;
using Sheaft.Options;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Controllers
{
    public class CatalogsController : ManageController
    {
        public CatalogsController(
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

            var query = _context.Catalogs.AsNoTracking();

            var requestUser = await GetRequestUserAsync(token);
            if (requestUser.IsImpersonating())
            {
                query = query.Where(p => p.ProducerId == requestUser.Id);
            }

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<CatalogViewModel>(_configurationProvider)
                .ToListAsync(token);

            ViewBag.Page = page;
            ViewBag.Take = take;

            return View(entities);
        }

        [HttpGet]
        public async Task<IActionResult> Add(CancellationToken token)
        {
            var requestUser = await GetRequestUserAsync(token);
            if (!requestUser.IsImpersonating())
                return RedirectToAction("Impersonate", "Account");

            return View(new CatalogViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CatalogViewModel model, IFormFile picture, CancellationToken token)
        {
            var requestUser = await GetRequestUserAsync(token);
            if (!requestUser.IsImpersonating())
            {
                ModelState.AddModelError("", "You must impersonate producer to create it.");
                return View(model);
            }

            var result = await _mediatr.Process(new CreateCatalogCommand(requestUser)
            {
                ProducerId = requestUser.Id,
                Name = model.Name,
                Kind = model.Kind,
                IsAvailable = model.Available,
                IsDefault = model.IsDefault,
                Products = new List<ProductPriceInputDto>()
            }, token);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("Edit", new {id = result.Data});
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUserAsync(token);
            if (!requestUser.IsImpersonating())
                return RedirectToAction("Impersonate", "Account");

            var entity = await _context.Catalogs
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<CatalogViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw SheaftException.NotFound();

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CatalogViewModel model, CancellationToken token)
        {
            var requestUser = await GetRequestUserAsync(token);
            if (!requestUser.IsImpersonating())
            {
                ModelState.AddModelError("", "You must impersonate catalog's producer to edit it.");
                return View(model);
            }

            var result = await _mediatr.Process(new UpdateCatalogCommand(requestUser)
            {
                CatalogId = model.Id,
                Name = model.Name,
                IsAvailable = model.Available,
                IsDefault = model.IsDefault,
                Products = model.Products?.Where(p => !p.Remove).Select(p => new ProductPriceInputDto
                               {ProductId = p.Id, WholeSalePricePerUnit = p.WholeSalePricePerUnit}) ??
                           new List<ProductPriceInputDto>(),
            }, token);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("Edit", new {model.Id});
        }

        [HttpGet]
        public async Task<IActionResult> AddProducts(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUserAsync(token);
            if (!requestUser.IsImpersonating())
                return RedirectToAction("Impersonate", "Account");

            var catalog = await _context.Catalogs.SingleAsync(c => c.Id == id, token);
            var productIds = catalog.Products.Select(p => p.ProductId);
            
            var products = await _context.Products
                .Where(p => p.ProducerId == requestUser.Id && !productIds.Contains(p.Id))
                .ToListAsync(token);

            ViewBag.Id = id;
            ViewBag.Products = products.Select(p => new SelectListItem(p.Name, p.Id.ToString("N"))).ToList();
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProducts(Guid id, List<Guid> products, CancellationToken token)
        {
            var catalog = await _context.Catalogs.SingleAsync(c => c.Id == id, token);
            var productIds = catalog.Products.Select(p => p.ProductId);
            
            var requestUser = await GetRequestUserAsync(token);
            if (!requestUser.IsImpersonating())
            {
                
                var pp = await _context.Products
                    .Where(p => p.ProducerId == requestUser.Id && !productIds.Contains(p.Id))
                    .ToListAsync(token);

                ViewBag.Id = id;
                ViewBag.Products = pp.Select(p => new SelectListItem(p.Name, p.Id.ToString("N"))).ToList();

                ModelState.AddModelError("", "You must impersonate catalog's producer to edit it.");
                return View();
            }

            var result = await _mediatr.Process(new AddOrUpdateProductsToCatalogCommand(requestUser)
            {
                CatalogId = id,
                Products = products?.Select(p => new ProductPriceInputDto {ProductId = p, WholeSalePricePerUnit = 0}) ??
                           new List<ProductPriceInputDto>(),
            }, token);

            if (!result.Succeeded)
            {
                var pp = await _context.Products
                    .Where(p => p.ProducerId == requestUser.Id && !productIds.Contains(p.Id))
                    .ToListAsync(token);

                ViewBag.Id = id;
                ViewBag.Products = pp.Select(p => new SelectListItem(p.Name, p.Id.ToString("N"))).ToList();

                ModelState.AddModelError("", result.Exception.Message);
                return View();
            }

            return RedirectToAction("Edit", new {id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new RestoreCatalogCommand(await GetRequestUserAsync(token))
            {
                CatalogId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new DeleteCatalogCommand(await GetRequestUserAsync(token))
            {
                CatalogId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Index");
        }
    }
}