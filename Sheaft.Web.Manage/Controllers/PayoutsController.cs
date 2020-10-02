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
    public class PayoutsController : ManageController
    {
        private readonly ILogger<PayoutsController> _logger;
        private RoutineOptions _routineOptions;

        public PayoutsController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider,
            ILogger<PayoutsController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
            _routineOptions = routineOptions.Value;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken token, TransactionStatus? status = null, int page = 0, int take = 10)
        {
            if (page < 0)
                page = 0;

            if (take > 100)
                take = 100;

            var query = _context.Payouts.Where(t => t.Status != TransactionStatus.Succeeded).AsNoTracking();

            var requestUser = await GetRequestUser(token);
            if (requestUser.IsImpersonating)
                query = query.Where(p => p.Author.Id == requestUser.Id);

            if (status != null)
                query = query.Where(t => t.Status == status);

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<PayoutViewModel>(_configurationProvider)
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

            var expiredDate = DateTimeOffset.UtcNow.AddMinutes(-_routineOptions.CheckNewPayoutsFromMinutes);
            var producersTransfers = await _context.Transfers
                .Get(t => t.Refund == null
                        && t.Status == TransactionStatus.Succeeded
                        && (t.Payout == null || t.Payout.Status == TransactionStatus.Expired)
                        && t.PurchaseOrder.Status == PurchaseOrderStatus.Delivered
                        && t.PurchaseOrder.DeliveredOn.HasValue && t.PurchaseOrder.DeliveredOn.Value < expiredDate)
                .Select(t => new ProducerPayoutViewModel { ProducerId = t.CreditedWallet.User.Id, Amount = t.Credited, TransferId = t.Id, Name = t.CreditedWallet.User.Name, Email = t.CreditedWallet.User.Email, Phone = t.CreditedWallet.User.Phone })
                .OrderBy(c => c.ProducerId)
                .Skip(page * take)
                .Take(take)
                .ToListAsync(token);

            var groupedProducers = producersTransfers.GroupBy(t => t.ProducerId);
            var entities = groupedProducers.Select(c => new ProducerPayoutsViewModel
            {
                Id = c.First().ProducerId,
                Email = c.First().Email,
                Name = c.First().Email,
                Phone = c.First().Email,
                Total = c.Sum(t => t.Amount),
                Transfers = c.Select(t => new ProducerTransferViewModel { Id = t.TransferId, Amount = t.Amount })
            });

            var submitted = (string)TempData["Submitted"];
            ViewBag.Submitted = !string.IsNullOrWhiteSpace(submitted) ? JsonConvert.DeserializeObject(submitted) : null;

            ViewBag.Page = page;
            ViewBag.Take = take;

            return View(entities);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePayout(ProducerPayoutsViewModel model, CancellationToken token)
        {
            if (!model.Transfers.Any())
                throw new InvalidOperationException();

            var requestUser = await GetRequestUser(token);
            _mediatr.Post(new CreatePayoutCommand(requestUser) { ProducerId = model.Id, TransferIds = model.Transfers.Select(t => t.Id) });

            TempData["Submitted"] = JsonConvert.SerializeObject(new EntityViewModel { Id = model.Id, Name = model.Name });
            return RedirectToAction("Todo");
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id, CancellationToken token)
        {
            var entity = await _context.Payouts
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<PayoutViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Expire(PayoutViewModel model, CancellationToken token)
        {
            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Process(new ExpirePayoutCommand(requestUser) { PayoutId = model.Id }, token);
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
