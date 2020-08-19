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
using Sheaft.Infrastructure.Interop;
using Sheaft.Manage.Models;
using Sheaft.Models.ViewModels;
using Sheaft.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Manage.Controllers
{
    public class DeliveryModesController : ManageController
    {
        private readonly ILogger<DeliveryModesController> _logger;

        public DeliveryModesController(
            IAppDbContext context,
            IMapper mapper,
            IMediator mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider,
            ILogger<DeliveryModesController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
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

            var query = _context.DeliveryModes.AsNoTracking();

            var requestUser = await GetRequestUser(token);
            if (requestUser.IsImpersonating)
            {
                query = query.Where(p => p.Producer.Id == requestUser.CompanyId);
            }

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<DeliveryModeViewModel>(_configurationProvider)
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

            var entity = await _context.DeliveryModes
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<DeliveryModeViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DeliveryModeViewModel model, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            if (!requestUser.IsImpersonating)
            {
                ModelState.AddModelError("", "You must impersonate packaging's producer to edit it.");
                return View(model);
            }

            var result = await _mediatr.Send(new UpdateDeliveryModeCommand(requestUser)
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
            var entity = await _context.DeliveryModes.SingleOrDefaultAsync(c => c.Id == id, token);

            _context.Remove(entity);
            await _context.SaveChangesAsync(token);

            TempData["Removed"] = entity.Name;
            return RedirectToAction("Index");
        }
    }
}
