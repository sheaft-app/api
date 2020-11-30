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
        public async Task<IActionResult> Create(Guid userId, CancellationToken token)
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
        public async Task<IActionResult> Create(BankAccountViewModel model, CancellationToken token)
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

            return RedirectToAction("Edit", new { id = result.Data });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var entity = await _context.BankAccounts
                .SingleOrDefaultAsync(c => c.Id == id, token);

            if (entity == null)
                throw new NotFoundException();

            ViewBag.Countries = await GetCountries(token);

            return View(new BankAccountViewModel
            {
                Id = entity.Id,
                Identifier = entity.Identifier,
                BIC = entity.BIC,
                IBAN = entity.IBAN,
                IsActive = entity.IsActive,
                Name = entity.Name,
                Owner = entity.Owner,
                OwnerId = entity.User.Id,
                Address = new AddressViewModel
                {
                    Line1 = entity.Line1,
                    Line2 = entity.Line2,
                    City = entity.City,
                    Zipcode = entity.Zipcode,
                    Country = entity.Country
                }
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BankAccountViewModel model, CancellationToken token)
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

            return RedirectToAction("Edit", new { id = model.Id });
        }
    }
}
