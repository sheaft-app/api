using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Exceptions;
using Sheaft.Application.Interop;
using Sheaft.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Commands;
using Sheaft.Application.Models;
using Sheaft.Domain.Enums;

namespace Sheaft.Web.Manage.Controllers
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
        public async Task<IActionResult> Index(CancellationToken token, OrderStatus? status = null, int page = 0, int take = 50)
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
                query = query.Where(q => q.Status == status);

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<OrderViewModel>(_configurationProvider)
                .ToListAsync(token);

            ViewBag.Page = page;
            ViewBag.Take = take;
            ViewBag.Status = status;

            return View(entities);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
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
            var result = await _mediatr.Process(new ExpireOrderCommand(await GetRequestUser(token))
            {
                OrderId = model.Id
            }, token);

            if (!result.Success)
                throw result.Exception;

            return RedirectToAction("Edit", new { model.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(OrderViewModel model, CancellationToken token)
        {
            var result = await _mediatr.Process(new ConfirmOrderCommand(await GetRequestUser(token))
            {
                OrderId = model.Id
            }, token);

            if (!result.Success)
                throw result.Exception;

            return RedirectToAction("Edit", new { model.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Fail(OrderViewModel model, CancellationToken token)
        {
            var result = await _mediatr.Process(new FailOrderCommand(await GetRequestUser(token))
            {
                OrderId = model.Id
            }, token);

            if (!result.Success)
                throw result.Exception;

            return RedirectToAction("Edit", new { model.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Retry(OrderViewModel model, CancellationToken token)
        {
            var result = await _mediatr.Process(new RetryOrderCommand(await GetRequestUser(token))
            {
                OrderId = model.Id
            }, token);

            if (!result.Success)
                throw result.Exception;

            return RedirectToAction("Edit", new { model.Id });
        }
    }
}
