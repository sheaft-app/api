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
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
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
            if (requestUser.IsImpersonating)
            {
                query = query.Where(p => p.Producer.Id == requestUser.Id);
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
            if (!requestUser.IsImpersonating)
                return RedirectToAction("Impersonate", "Account");

            ViewBag.Tags = await GetTags(token);
            ViewBag.Returnables = await GetReturnables(requestUser, token);

            return View(new ProductViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ProductViewModel model, IFormFile picture, CancellationToken token)
        {
            var requestUser = await GetRequestUserAsync(token);
            if (!requestUser.IsImpersonating)
            {
                ViewBag.Tags = await GetTags(token);
                ViewBag.Returnables = await GetReturnables(requestUser, token);

                ModelState.AddModelError("", "You must impersonate producer to create it.");
                return View(model);
            }

            if (picture != null)
            {
                using (var ms = new MemoryStream())
                {
                    await picture.CopyToAsync(ms, token);
                    model.Picture = Convert.ToBase64String(ms.ToArray());
                }
            }

            var result = await _mediatr.Process(new CreateProductCommand(requestUser)
            {
                ProducerId = requestUser.Id,
                Description = model.Description,
                Name = model.Name,
                Available = model.Available,
                VisibleToStores = model.VisibleToStores,
                VisibleToConsumers = model.VisibleToConsumers,
                ReturnableId = model.ReturnableId,
                Picture = !string.IsNullOrWhiteSpace(model.Picture) ? new PictureSourceDto{Original = model.Picture} : null,
                QuantityPerUnit = model.QuantityPerUnit,
                Reference = model.Reference,
                Tags = model.Tags,
                Unit = model.Unit,
                Conditioning = model.Conditioning,
                Vat = model.Vat,
                Weight = model.Weight,
                WholeSalePricePerUnit = model.WholeSalePricePerUnit
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
            if (!requestUser.IsImpersonating)
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
        public async Task<IActionResult> Edit(ProductViewModel model, IFormFile picture, CancellationToken token)
        {
            var requestUser = await GetRequestUserAsync(token);
            if (!requestUser.IsImpersonating)
            {
                ViewBag.Tags = await GetTags(token);
                ViewBag.Returnables = await GetReturnables(requestUser, token);

                ModelState.AddModelError("", "You must impersonate product's producer to edit it.");
                return View(model);
            }

            if (picture != null)
            {
                using (var ms = new MemoryStream())
                {
                    await picture.CopyToAsync(ms, token);
                    model.Picture = Convert.ToBase64String(ms.ToArray());
                }
            }

            var result = await _mediatr.Process(new UpdateProductCommand(requestUser)
            {
                ProductId = model.Id,
                Description = model.Description,
                Name = model.Name,
                Available = model.Available,
                VisibleToStores = model.VisibleToStores,
                VisibleToConsumers = model.VisibleToConsumers,
                ReturnableId = model.ReturnableId,
                Picture = !string.IsNullOrWhiteSpace(model.Picture) ? new PictureSourceDto{Original = model.Picture} : null,
                QuantityPerUnit = model.QuantityPerUnit,
                Reference = model.Reference,
                Tags = model.Tags,
                Conditioning = model.Conditioning,
                Unit = model.Unit,
                Vat = model.Vat,
                Weight = model.Weight,
                WholeSalePricePerUnit = model.WholeSalePricePerUnit
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
                .Where(c => c.Producer.Id == requestUser.Id && !c.RemovedOn.HasValue)
                .ProjectTo<ReturnableViewModel>(_configurationProvider)
                .ToListAsync(token);
        }
    }
}