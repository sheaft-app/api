using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Commands;
using Sheaft.Core.Extensions;
using Sheaft.Infrastructure.Interop;
using Sheaft.Interop.Enums;
using Sheaft.Manage.Models;
using Sheaft.Models.Dto;
using Sheaft.Models.Inputs;
using Sheaft.Options;
using System;
using System.Collections.Generic;
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
        private readonly IMediator _mediatr;
        private readonly RoleOptions _roleOptions;
        private readonly IConfigurationProvider _configurationProvider;

        public CompaniesController(
            IAppDbContext context,
            IMapper mapper,
            IMediator mediatr,
            IConfigurationProvider configurationProvider,
            IOptionsSnapshot<RoleOptions> roleOptions,
            ILogger<CompaniesController> logger)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _mediatr = mediatr;
            _roleOptions = roleOptions.Value;
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
            var requestUser = User.ToIdentityUser(HttpContext.TraceIdentifier);

            var result = await _mediatr.Send(new UpdateCompanyCommand(requestUser)
            {
                Id = model.Id,
                Address = _mapper.Map<AddressInput>(model.Address),
                AppearInBusinessSearchResults = model.AppearInBusinessSearchResults,
                Description = model.Description,
                Email = model.Email,
                Name = model.Name,
                Phone = model.Phone,
                Siret = model.Siret,
                VatIdentifier = model.VatIdentifier
            }, token);

            if(!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            var entity = await _context.Companies.SingleOrDefaultAsync(c => c.Id == model.Id, token);
            if (entity.Kind != model.Kind)
            {
                var user = await _context.Users.SingleOrDefaultAsync(c => c.UserType == UserKind.Owner && c.Company.Id == entity.Id, token);
                var roles = new List<string> { _roleOptions.Owner.Value };
                switch (model.Kind)
                {
                    case ProfileKind.Producer:
                        roles.Add(_roleOptions.Producer.Value);
                        break;
                    case ProfileKind.Store:
                        roles.Add(_roleOptions.Store.Value);
                        break;
                }

                var res = await _mediatr.Send(new ChangeUserRolesCommand(requestUser)
                {
                    Id = user.Id,
                    Roles = roles
                }, token);

                if (!res.Success)
                {
                    ModelState.AddModelError("", result.Exception.Message);
                    return View(model);
                }
            }

            TempData["Edited"] = JsonConvert.SerializeObject(new EditViewModel { Id = model.Id, Name = model.Name });
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

            TempData["Removed"] = entity.Name;
            return RedirectToAction("Index");
        }
    }
}
