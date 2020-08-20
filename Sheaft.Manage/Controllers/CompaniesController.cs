using AutoMapper;
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
using Sheaft.Exceptions;
using Sheaft.Infrastructure.Interop;
using Sheaft.Interop.Enums;
using Sheaft.Manage.Models;
using Sheaft.Models.Inputs;
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
    public class CompaniesController : ManageController
    {
        private readonly ILogger<CompaniesController> _logger;

        public CompaniesController(
            IAppDbContext context,
            IMapper mapper,
            IMediator mediatr,
            IConfigurationProvider configurationProvider,
            IOptionsSnapshot<RoleOptions> roleOptions,
            ILogger<CompaniesController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
            _logger = logger;
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

            var requestUser = await GetRequestUser(token);
            if (requestUser.IsImpersonating)
            {
                query = query.Where(p => p.Id == requestUser.CompanyId);
            }

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<CompanyViewModel>(_configurationProvider)
                .ToListAsync(token);

            var edited = (string)TempData["Edited"];
            ViewBag.Edited = !string.IsNullOrWhiteSpace(edited) ? JsonConvert.DeserializeObject(edited) : null;

            var restored = (string)TempData["Restored"];
            ViewBag.Restored = !string.IsNullOrWhiteSpace(restored) ? JsonConvert.DeserializeObject(restored) : null;

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
                .ProjectTo<CompanyViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            ViewBag.Tags = await GetTags(token);
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CompanyViewModel model, IFormFile picture, CancellationToken token)
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
                VatIdentifier = model.VatIdentifier,
                Tags = model.Tags,
                Picture = model.Picture
            }, token);

            if (!result.Success)
            {
                ViewBag.Tags = await GetTags(token);

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
                    ViewBag.Tags = await GetTags(token);

                    ModelState.AddModelError("", result.Exception.Message);
                    return View(model);
                }
            }

            TempData["Edited"] = JsonConvert.SerializeObject(new EntityViewModel { Id = model.Id, Name = model.Name });
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token)
        {
            var entity = await _context.Companies.SingleOrDefaultAsync(c => c.Id == id, token);
            var name = entity.Name;

            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Send(new DeleteCompanyCommand(requestUser)
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
    }
}
