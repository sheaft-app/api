using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Mediatr.Bank.Commands;
using Sheaft.Options;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Controllers
{
    public class BankAccountsController : ManageController
    {
        public BankAccountsController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider)
            : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid userId, CancellationToken token)
        {
            var entity = await _context.Users.OfType<Domain.Business>()
                .AsNoTracking()
                .Where(c => c.Id == userId)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw SheaftException.NotFound();

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
            var result = await _mediatr.Process(new CreateBankAccountCommand(await GetRequestUserAsync(token))
            {
                UserId = model.OwnerId,
                Address = _mapper.Map<AddressDto>(model.Address),
                BIC = model.BIC,
                IBAN = model.IBAN,
                Name = model.Name,
                Owner = model.Owner
            }, token);

            if (!result.Succeeded)
            {
                ViewBag.Countries = await GetCountries(token);
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("Edit", new {id = result.Data});
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var entity = await _context.BankAccounts
                .SingleOrDefaultAsync(c => c.Id == id, token);

            if (entity == null)
                throw SheaftException.NotFound();

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
                OwnerId = entity.UserId,
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
            var result = await _mediatr.Process(new UpdateBankAccountCommand(await GetRequestUserAsync(token))
            {
                BankAccountId = model.Id,
                Address = _mapper.Map<AddressDto>(model.Address),
                BIC = model.BIC,
                IBAN = model.IBAN,
                Name = model.Name,
                Owner = model.Owner
            }, token);

            if (!result.Succeeded)
            {
                ViewBag.Countries = await GetCountries(token);
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("Edit", new {id = model.Id});
        }
    }
}