using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Commands;
using Sheaft.Core.Extensions;
using Sheaft.Infrastructure.Interop;
using Sheaft.Manage.Models;
using Sheaft.Models.Dto;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Manage.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediatr;
        private readonly IConfigurationProvider _configurationProvider;

        public ProductsController(
            IAppDbContext context,
            IMapper mapper,
            IMediator mediatr,
            IConfigurationProvider configurationProvider,
            ILogger<ProductsController> logger)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _mediatr = mediatr;
            _configurationProvider = configurationProvider;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken token, int page = 0, int take = 10)
        {
            if (page < 0)
                page = 0;

            if (take > 100)
                take = 100;

            var query = _context.Products.AsNoTracking();

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<ProductDto>(_configurationProvider)
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
            var entity = await _context.Products
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<ProductDto>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductDto model, CancellationToken token)
        {
            var requestUser = User.ToIdentityUser(HttpContext.TraceIdentifier);

            var result = await _mediatr.Send(new UpdateProductCommand(requestUser)
            {
                Id = model.Id,
                Description = model.Description,
                Name = model.Name,
            }, token);

            if (!result.Success)
            {
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
    }
}
