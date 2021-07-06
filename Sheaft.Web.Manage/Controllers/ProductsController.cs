using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Mediatr.Catalog.Commands;
using Sheaft.Mediatr.Product.Commands;
using Sheaft.Options;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Controllers
{
    public class ProductsController : ManageController
    {
        public ProductsController(
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

            var query = _context.Products.AsNoTracking();

            var requestUser = await GetRequestUserAsync(token);
            if (requestUser.IsImpersonating())
            {
                query = query.Where(p => p.ProducerId == requestUser.Id);
            }

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<ProductViewModel>(_configurationProvider)
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

            ViewBag.Tags = await GetTags(token);
            ViewBag.Returnables = await GetReturnables(requestUser, token);

            return View(new ProductViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ProductViewModel model, List<IFormFile> newPictures, CancellationToken token)
        {
            var requestUser = await GetRequestUserAsync(token);
            if (!requestUser.IsImpersonating())
            {
                ViewBag.Tags = await GetTags(token);
                ViewBag.Returnables = await GetReturnables(requestUser, token);

                ModelState.AddModelError("", "You must impersonate producer to create it.");
                return View(model);
            }

            var images = model.Pictures?.Where(p => !p.Remove).Select(p => new PictureInputDto{Id = p.Id, Data = p.Url, Position = p.Position})?.ToList() ?? new List<PictureInputDto>();
            if (newPictures != null && newPictures.Any())
            {
                foreach (var picture in newPictures)
                {
                    using (var ms = new MemoryStream())
                    {
                        await picture.CopyToAsync(ms, token);
                        images.Add(new PictureInputDto{Data = Convert.ToBase64String(ms.ToArray()), Position = images.Count});
                    }
                }
            }

            var result = await _mediatr.Process(new CreateProductCommand(requestUser)
            {
                ProducerId = requestUser.Id,
                Description = model.Description,
                Name = model.Name,
                Available = model.Available,
                ReturnableId = model.ReturnableId,
                QuantityPerUnit = model.QuantityPerUnit,
                Reference = model.Reference,
                Tags = model.Tags,
                Unit = model.Unit,
                Conditioning = model.Conditioning,
                Vat = model.Vat,
                Weight = model.Weight,
                Pictures = images,
                Catalogs = model.CatalogsPrices?.Where(c => !c.Remove).Select(c => new CatalogPriceInputDto{CatalogId = c.Id, WholeSalePricePerUnit = c.WholeSalePricePerUnit}) ?? new List<CatalogPriceInputDto>()
            }, token);

            if (!result.Succeeded)
            {
                ViewBag.Tags = await GetTags(token);
                ViewBag.Returnables = await GetReturnables(requestUser, token);

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

            var entity = await _context.Products
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<ProductViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw SheaftException.NotFound();

            ViewBag.Tags = await GetTags(token);
            ViewBag.Returnables = await GetReturnables(requestUser, token);

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel model, List<IFormFile> newPictures, CancellationToken token)
        {
            var requestUser = await GetRequestUserAsync(token);
            if (!requestUser.IsImpersonating())
            {
                ViewBag.Tags = await GetTags(token);
                ViewBag.Returnables = await GetReturnables(requestUser, token);

                ModelState.AddModelError("", "You must impersonate product's producer to edit it.");
                return View(model);
            }

            var images = model.Pictures?.Where(p => !p.Remove).Select(p => new PictureInputDto{Id = p.Id, Data = p.Url, Position = p.Position})?.ToList() ?? new List<PictureInputDto>();
            if (newPictures != null && newPictures.Any())
            {
                foreach (var picture in newPictures)
                {
                    using (var ms = new MemoryStream())
                    {
                        await picture.CopyToAsync(ms, token);
                        images.Add(new PictureInputDto{Data = Convert.ToBase64String(ms.ToArray()), Position = images.Count});
                    }
                }
            }

            var result = await _mediatr.Process(new UpdateProductCommand(requestUser)
            {
                ProductId = model.Id,
                Description = model.Description,
                Name = model.Name,
                Available = model.Available,
                ReturnableId = model.ReturnableId,
                QuantityPerUnit = model.QuantityPerUnit,
                Reference = model.Reference,
                Tags = model.Tags,
                Conditioning = model.Conditioning,
                Unit = model.Unit,
                Vat = model.Vat,
                Weight = model.Weight,
                Pictures = images,
                Catalogs = model.CatalogsPrices?.Where(c => !c.Remove).Select(c => new CatalogPriceInputDto{CatalogId = c.Id, WholeSalePricePerUnit = c.WholeSalePricePerUnit}) ?? new List<CatalogPriceInputDto>()
            }, token);

            if (!result.Succeeded)
            {
                ViewBag.Tags = await GetTags(token);
                ViewBag.Returnables = await GetReturnables(requestUser, token);

                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("Edit", new {model.Id});
        }
        
        

        [HttpGet]
        public async Task<IActionResult> AddCatalogs(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUserAsync(token);
            if (!requestUser.IsImpersonating())
                return RedirectToAction("Impersonate", "Account");

            var product = await _context.Products.SingleAsync(c => c.Id == id, token);
            var catalogIds = product.CatalogsPrices.Select(p => p.CatalogId);
            
            var catalogs = await _context.Catalogs
                .Where(p => p.ProducerId == requestUser.Id && !catalogIds.Contains(p.Id))
                .ToListAsync(token);

            ViewBag.Id = id;
            ViewBag.Catalogs = catalogs.Select(p => new SelectListItem(p.Name, p.Id.ToString("N"))).ToList();
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCatalogs(Guid id, List<Guid> catalogs, CancellationToken token)
        {
            var product = await _context.Products.SingleAsync(c => c.Id == id, token);
            var catalogIds = product.CatalogsPrices.Select(p => p.CatalogId);
            
            var requestUser = await GetRequestUserAsync(token);
            if (!requestUser.IsImpersonating())
            {
                var pp = await _context.Catalogs
                    .Where(p => p.ProducerId == requestUser.Id && !catalogIds.Contains(p.Id))
                    .ToListAsync(token);

                ViewBag.Id = id;
                ViewBag.Catalogs = pp.Select(p => new SelectListItem(p.Name, p.Id.ToString("N"))).ToList();

                ModelState.AddModelError("", "You must impersonate catalog's producer to edit it.");
                return View();
            }

            Result result = null;
            foreach (var catalogId in catalogs)
            {
                result = await _mediatr.Process(new AddOrUpdateProductsToCatalogCommand(requestUser)
                {
                    CatalogId = catalogId,
                    Products = new List<ProductPriceInputDto>{new ProductPriceInputDto{ProductId = id, WholeSalePricePerUnit = 0}},
                }, token);

                if (!result.Succeeded)
                    break;
            }

            if (!result.Succeeded)
            {
                var pp = await _context.Catalogs
                    .Where(p => p.ProducerId == requestUser.Id && !catalogIds.Contains(p.Id))
                    .ToListAsync(token);

                ViewBag.Id = id;
                ViewBag.Catalogs = pp.Select(p => new SelectListItem(p.Name, p.Id.ToString("N"))).ToList();

                ModelState.AddModelError("", result.Exception.Message);
                return View();
            }

            return RedirectToAction("Edit", new {id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IEnumerable<IFormFile> products, CancellationToken token)
        {
            var requestUser = await GetRequestUserAsync(token);
            foreach (var formFile in products)
            {
                if (formFile.Length == 0)
                    continue;

                using (var stream = new MemoryStream())
                {
                    await formFile.CopyToAsync(stream, token);
                    var result = await _mediatr.Process(
                        new QueueImportProductsCommand(requestUser)
                        {
                            ProducerId = requestUser.Id, NotifyOnUpdates = false, FileName = formFile.FileName,
                            FileStream = stream.ToArray()
                        }, token);
                    if (!result.Succeeded)
                        return BadRequest(result);
                }
            }

            return RedirectToAction("Index", "Jobs");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new RestoreProductCommand(await GetRequestUserAsync(token))
            {
                ProductId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new DeleteProductCommand(await GetRequestUserAsync(token))
            {
                ProductId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Index");
        }

        private async Task<List<ReturnableViewModel>> GetReturnables(RequestUser requestUser, CancellationToken token)
        {
            return await _context.Returnables
                .Where(c => c.ProducerId == requestUser.Id && !c.RemovedOn.HasValue)
                .ProjectTo<ReturnableViewModel>(_configurationProvider)
                .ToListAsync(token);
        }
    }
}