using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interop;
using Sheaft.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Models;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using System.Collections.Generic;
using Sheaft.Exceptions;

namespace Sheaft.Web.Manage.Controllers
{
    public class BankAccountsController : ManageController
    {
        private readonly ILogger<BankAccountsController> _logger;

        public BankAccountsController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider,
            ILogger<BankAccountsController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> CreateBankAccount(Guid userId, CancellationToken token)
        {
            var entity = await _context.Users.OfType<Business>()
                .AsNoTracking()
                .Where(c => c.Id == userId)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            ViewBag.Countries = await GetCountries(token);
            return View(new BankAccountViewModel
            {
                OwnerId = entity.Id,
                IsActive = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBankAccount(BankAccountViewModel model, CancellationToken token)
        {
            var result = await _mediatr.Process(new CreateBankAccountCommand(await GetRequestUser(token))
            {
                UserId = model.OwnerId,
                Address = _mapper.Map<AddressInput>(model.Address),
                BIC = model.BIC,
                IBAN = model.IBAN,
                Name = model.Name,
                Owner = model.Owner
            }, token);

            if (!result.Success)
            {
                ViewBag.Countries = await GetCountries(token);
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("Edit", new { id = model.OwnerId });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateBankAccount(Guid bankAccountId, CancellationToken token)
        {
            var entity = await _context.BankAccounts.Get(c => c.Id == bankAccountId)
                .ProjectTo<BankAccountViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            ViewBag.Countries = await GetCountries(token);
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBankAccount(BankAccountViewModel model, CancellationToken token)
        {
            var result = await _mediatr.Process(new UpdateBankAccountCommand(await GetRequestUser(token))
            {
                Id = model.Id,
                Address = _mapper.Map<AddressInput>(model.Address),
                BIC = model.BIC,
                IBAN = model.IBAN,
                Name = model.Name,
                Owner = model.Owner
            }, token);

            if (!result.Success)
            {
                ViewBag.Countries = await GetCountries(token);
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("Edit", new { id = model.OwnerId });
        }
    }
}
