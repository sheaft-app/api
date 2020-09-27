using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Exceptions;
using Sheaft.Application.Interop;
using Sheaft.Manage.Models;
using Sheaft.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Domain.Enums;
using Sheaft.Application.Commands;
using Sheaft.Application.Models;

namespace Sheaft.Manage.Controllers
{
    public class TransfersController : ManageController
    {
        private readonly ILogger<TransfersController> _logger;

        public TransfersController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider,
            ILogger<TransfersController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken token, TransactionStatus? status = null, int page = 0, int take = 10)
        {
            if (page < 0)
                page = 0;

            if (take > 100)
                take = 100;

            var query = _context.Transfers.Where(t => t.Status != TransactionStatus.Succeeded).AsNoTracking();

            var requestUser = await GetRequestUser(token);
            if (requestUser.IsImpersonating)
                query = query.Where(p => p.Author.Id == requestUser.Id);

            if (status != null)
                query = query.Where(t => t.Status == status);

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<TransferViewModel>(_configurationProvider)
                .ToListAsync(token);

            var edited = (string)TempData["Edited"];
            ViewBag.Edited = !string.IsNullOrWhiteSpace(edited) ? JsonConvert.DeserializeObject(edited) : null;

            var restored = (string)TempData["Restored"];
            ViewBag.Restored = !string.IsNullOrWhiteSpace(restored) ? JsonConvert.DeserializeObject(restored) : null;

            ViewBag.Removed = TempData["Removed"];
            ViewBag.Page = page;
            ViewBag.Take = take;
            ViewBag.Status = status;

            return View(entities);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id, CancellationToken token)
        {
            var entity = await _context.Transfers
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<TransferViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Expire(TransferViewModel model, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Process(new ExpireTransferCommand(requestUser) { TransferId = model.Id }, token);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return View("Details", model);
            }

            TempData["Edited"] = JsonConvert.SerializeObject(new EntityViewModel { Id = model.Id, Name = model.Identifier });
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unblock(TransferViewModel model, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Process(new UnblockTransferCommand(requestUser) { TransferId = model.Id }, token);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return View("Details", model);
            }

            TempData["Edited"] = JsonConvert.SerializeObject(new EntityViewModel { Id = model.Id, Name = model.Identifier });
            return RedirectToAction("Index");
        }
    }
}
