using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sheaft.Infrastructure.Interop;
using Sheaft.Interop.Enums;
using Sheaft.Models.Dto;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Manage.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ILogger<CompaniesController> _logger;
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfigurationProvider _configurationProvider;

        public CompaniesController(
            IAppDbContext context,
            IMapper mapper,
            IConfigurationProvider configurationProvider,
            ILogger<CompaniesController> logger)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _configurationProvider = configurationProvider;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken token, ProfileKind? kind = null, int page = 0, int take = 10)
        {
            if (page < 0)
                page = 0;

            if (take > 100)
                take = 100;

            var query = _context.Companies.AsNoTracking();

            if (kind != null)
                query = query.Where(c => c.Kind == kind);

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<CompanyDto>(_configurationProvider)
                .ToListAsync(token);

            ViewBag.Removed = TempData["Removed"];
            ViewBag.Edited = TempData["Edited"];
            ViewBag.Page = page;
            ViewBag.Take = take;
            ViewBag.Kind = kind;

            return View(entities);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var entity = await _context.Companies
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<CompanyDto>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CompanyDto model, CancellationToken token)
        {
            var entity = await _context.Companies.SingleOrDefaultAsync(c => c.Id == model.Id, token);

            _context.Update(entity);
            await _context.SaveChangesAsync(token);

            TempData["Edited"] = model;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token)
        {
            var entity = await _context.Companies.SingleOrDefaultAsync(c => c.Id == id, token);

            _context.Remove(entity);
            await _context.SaveChangesAsync(token);

            TempData["Removed"] = _mapper.Map<CompanyDto>(entity);
            return RedirectToAction("Index");
        }
    }
}
