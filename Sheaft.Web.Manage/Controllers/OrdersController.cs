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
using Sheaft.Application.Commands;
using Sheaft.Application.Models;
using Sheaft.Domain.Enums;

namespace Sheaft.Manage.Controllers
{
    public class OrdersController : ManageController
    {
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider,
            ILogger<OrdersController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken token, OrderStatus? status = null, int page = 0, int take = 10)
        {
            if (page < 0)
                page = 0;

            if (take > 100)
                take = 100;

            var query = _context.Orders.AsNoTracking();

            var requestUser = await GetRequestUser(token);
            if (requestUser.IsImpersonating)
                query = query.Where(p => p.User.Id == requestUser.Id);

            if (status != null)
                query.Where(q => q.Status == status);

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<OrderViewModel>(_configurationProvider)
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
            var entity = await _context.Orders
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<OrderViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Expire(OrderViewModel model, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Process(new ExpireOrderCommand(requestUser) { OrderId = model.Id }, token);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return View("Details", model);
            }

            TempData["Edited"] = JsonConvert.SerializeObject(new EntityViewModel { Id = model.Id, Name = model.Payin?.Identifier });
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(OrderViewModel model, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Process(new ConfirmOrderCommand(requestUser) { OrderId = model.Id }, token);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return View("Details", model);
            }

            TempData["Edited"] = JsonConvert.SerializeObject(new EntityViewModel { Id = model.Id, Name = model.Payin?.Identifier });
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Fail(OrderViewModel model, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Process(new FailOrderCommand(requestUser) { OrderId = model.Id }, token);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return View("Details", model);
            }

            TempData["Edited"] = JsonConvert.SerializeObject(new EntityViewModel { Id = model.Id, Name = model.Payin?.Identifier });
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Retry(OrderViewModel model, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Process(new RetryOrderCommand(requestUser) { OrderId = model.Id }, token);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return View("Details", model);
            }

            TempData["Edited"] = JsonConvert.SerializeObject(new EntityViewModel { Id = model.Id, Name = model.Payin?.Identifier });
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unblock(OrderViewModel model, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Process(new UnblockOrderCommand(requestUser) { OrderId = model.Id }, token);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return View("Details", model);
            }

            TempData["Edited"] = JsonConvert.SerializeObject(new EntityViewModel { Id = model.Id, Name = model.Payin?.Identifier });
            return RedirectToAction("Index");
        }
    }
}
