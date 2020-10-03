using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Exceptions;
using Sheaft.Application.Interop;
using Sheaft.Web.Manage.Models;
using Sheaft.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Domain.Enums;
using Sheaft.Application.Commands;
using Sheaft.Application.Models;

namespace Sheaft.Web.Manage.Controllers
{
    public class PayinsController : ManageController
    {
        private readonly ILogger<PayinsController> _logger;
        private RoutineOptions _routineOptions;

        public PayinsController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider,
            ILogger<PayinsController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
            _logger = logger;
            _routineOptions = routineOptions.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken token, TransactionStatus? status = null, int page = 0, int take = 10)
        {
            if (page < 0)
                page = 0;

            if (take > 100)
                take = 100;

            var query = _context.Payins.AsNoTracking();

            var requestUser = await GetRequestUser(token);
            if (requestUser.IsImpersonating)
                query = query.Where(p => p.Author.Id == requestUser.Id);

            if (status != null)
                query = query.Where(t => t.Status == status);

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<PayinViewModel>(_configurationProvider)
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
        public async Task<IActionResult> Todo(CancellationToken token, int page = 0, int take = 10)
        {
            if (page < 0)
                page = 0;

            if (take > 100)
                take = 100;

            var expiredDate = DateTimeOffset.UtcNow.AddMinutes(-_routineOptions.CheckNewPayinRefundsFromMinutes);
            var ordersToRefund = await _context.Orders
                .Get(c =>
                        c.Payin != null
                        && !c.Payin.SkipBackgroundProcessing
                        && c.Payin.Status == TransactionStatus.Succeeded
                        && (c.Payin.Refund == null || c.Payin.Status == TransactionStatus.Expired)
                        && c.PurchaseOrders.All(po => po.Status >= PurchaseOrderStatus.Delivered)
                        && c.PurchaseOrders.Any(po => po.WithdrawnOn.HasValue
                                                    && (po.Transfer == null
                                                    || (po.Transfer.Status == TransactionStatus.Succeeded && po.Transfer.Refund != null && po.Transfer.Refund.Status == TransactionStatus.Succeeded)))
                        && c.PurchaseOrders.Max(po => po.WithdrawnOn) < expiredDate, true)
                .OrderBy(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<OrderViewModel>(_configurationProvider)
                .ToListAsync(token);

            var submitted = (string)TempData["Submitted"];
            ViewBag.Submitted = !string.IsNullOrWhiteSpace(submitted) ? JsonConvert.DeserializeObject(submitted) : null;

            ViewBag.Page = page;
            ViewBag.Take = take;

            return View(ordersToRefund);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePayinRefund(Guid id, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            _mediatr.Post(new CreatePayinRefundCommand(requestUser) { OrderId = id });

            TempData["Submitted"] = JsonConvert.SerializeObject(new EntityViewModel { Id = id, Name = id.ToString("N") });
            return RedirectToAction("Todo");
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id, CancellationToken token)
        {
            var entity = await _context.Payins
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<PayinViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Expire(PayinViewModel model, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Process(new ExpirePayinCommand(requestUser) { PayinId = model.Id }, token);
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
