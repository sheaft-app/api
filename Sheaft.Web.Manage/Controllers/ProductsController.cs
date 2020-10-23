using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Commands;
using Sheaft.Exceptions;
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Web.Manage.Models;
using Sheaft.Application.Models;
using Sheaft.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Web.Manage.Controllers
{
    public class ProductsController : ManageController
    {
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
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
                query = query.Where(p => p.Producer.Id == requestUser.Id);
            }

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<ProductViewModel>(_configurationProvider)
                .ToListAsync(token);

            var edited = (string)TempData["Edited"];
            ViewBag.Edited = !string.IsNullOrWhiteSpace(edited) ? JsonConvert.DeserializeObject(edited) : null;

            var restored = (string)TempData["Restored"];
            ViewBag.Restored = !string.IsNullOrWhiteSpace(restored) ? JsonConvert.DeserializeObject(restored) : null;

            ViewBag.Removed = TempData["Removed"];
            ViewBag.Page = page;
            ViewBag.Take = take;

            return View(entities);
        }

        [HttpGet]
        public async Task<IActionResult> Add(CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
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
            var requestUser = await GetRequestUser(token);
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
                    picture.CopyTo(ms);
                    model.Picture = Convert.ToBase64String(ms.ToArray());
                }
            }

            var result = await _mediatr.Process(new CreateProductCommand(requestUser)
            {
                Description = model.Description,
                Name = model.Name,
                Available = model.Available,
                ReturnableId = model.ReturnableId,
                Picture = model.Picture,
                QuantityPerUnit = model.QuantityPerUnit,
                Reference = model.Reference,
                Tags = model.Tags,
                Unit = model.Unit,
                Conditioning = model.Conditioning,
                Vat = model.Vat,
                Weight = model.Weight,
                WholeSalePricePerUnit = model.WholeSalePricePerUnit
            }, token);

            if (!result.Success)
            {
                ViewBag.Tags = await GetTags(token);
                ViewBag.Returnables = await GetReturnables(requestUser, token);

                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("Edit", new { id = result.Data });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            if (!requestUser.IsImpersonating)
                return RedirectToAction("Impersonate", "Account");

            var entity = await _context.Products
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<ProductViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            ViewBag.Tags = await GetTags(token);
            ViewBag.Returnables = await GetReturnables(requestUser, token);

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
                ViewBag.Returnables = await GetReturnables(requestUser, token);

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

            var result = await _mediatr.Process(new UpdateProductCommand(requestUser)
            {
                Id = model.Id,
                Description = model.Description,
                Name = model.Name,
                Available = model.Available,
                ReturnableId = model.ReturnableId,
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
                ViewBag.Returnables = await GetReturnables(requestUser, token);

                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            TempData["Edited"] = JsonConvert.SerializeObject(new EntityViewModel { Id = model.Id, Name = model.Name });
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IEnumerable<IFormFile> products, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            foreach (var formFile in products)
            {
                if (formFile.Length == 0)
                    continue;

                using (var stream = new MemoryStream())
                {
                    await formFile.CopyToAsync(stream, token);
                    var result = await _mediatr.Process(new QueueImportProductsCommand(requestUser) { Id = requestUser.Id, FileName = formFile.FileName, FileStream = stream.ToArray() }, token);
                    if (!result.Success)
                        return BadRequest(result);
                }
            }

            return RedirectToAction("Index", "Jobs");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token)
        {
            var entity = await _context.Products.SingleOrDefaultAsync(c => c.Id == id, token);
            var name = entity.Name;

            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Process(new DeleteProductCommand(requestUser)
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
            var entity = await _context.Products.SingleOrDefaultAsync(c => c.Id == id, token);
            var name = entity.Name;

            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Process(new RestoreProductCommand(requestUser)
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

        private async Task<List<ReturnableViewModel>> GetReturnables(RequestUser requestUser, CancellationToken token)
        {
            return await _context.Returnables
                            .Where(c => c.Producer.Id == requestUser.Id && !c.RemovedOn.HasValue)
                            .ProjectTo<ReturnableViewModel>(_configurationProvider)
                            .ToListAsync(token);
        }
    }
}
