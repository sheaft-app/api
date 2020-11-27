using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Exceptions;
using Sheaft.Application.Interop;
using Sheaft.Application.Models;
using Sheaft.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Web.Manage.Controllers
{
    public class ProducersController : ManageController
    {
        private readonly ILogger<ProducersController> _logger;

        public ProducersController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IConfigurationProvider configurationProvider,
            IOptionsSnapshot<RoleOptions> roleOptions,
            ILogger<ProducersController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
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

            var query = _context.Users.OfType<Producer>().AsNoTracking();

            var requestUser = await GetRequestUser(token);
            if (requestUser.IsImpersonating)
                query = query.Where(p => p.Id == requestUser.Id);

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<ProducerViewModel>(_configurationProvider)
                .ToListAsync(token);

            ViewBag.Page = page;
            ViewBag.Take = take;
            return View(entities);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var entity = await _context.Users.OfType<Producer>()
                .Where(c => c.Id == id)
                .ProjectTo<ProducerViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            ViewBag.LegalsId = (await _context.FindSingleAsync<Legal>(c => c.User.Id == id, token))?.Id;
            ViewBag.BankAccountId = (await _context.FindSingleAsync<BankAccount>(c => c.User.Id == id, token))?.Id;

            ViewBag.Tags = await GetTags(token);
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProducerViewModel model, IFormFile picture, CancellationToken token)
        {
            if (picture != null)
            {
                using (var ms = new MemoryStream())
                {
                    picture.CopyTo(ms);
                    model.Picture = Convert.ToBase64String(ms.ToArray());
                }
            }

            var producer = await _context.Users.OfType<Producer>().SingleOrDefaultAsync(c => c.Id == model.Id, token);
            var result = await _mediatr.Process(new UpdateProducerCommand(await GetRequestUser(token))
            {
                Id = model.Id,
                Address = _mapper.Map<FullAddressInput>(model.Address),
                OpenForNewBusiness = model.OpenForNewBusiness,
                Description = model.Description,
                Email = model.Email,
                Name = model.Name,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Kind = model.Kind,
                Phone = model.Phone,
                Tags = producer.Tags.Select(t => t.Tag.Id),
                Picture = model.Picture,
                NotSubjectToVat = model.NotSubjectToVat
            }, token);

            if (!result.Success)
            {
                ViewBag.LegalsId = (await _context.FindSingleAsync<Legal>(c => c.User.Id == model.Id, token))?.Id;
                ViewBag.BankAccountId = (await _context.FindSingleAsync<BankAccount>(c => c.User.Id == model.Id, token))?.Id;
                ViewBag.Tags = await GetTags(token);

                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("Edit", new { model.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new RemoveUserCommand(await GetRequestUser(token))
            {
                Id = id
            }, token);

            if (!result.Success)
                throw result.Exception;

            return RedirectToAction("Index");
        }
    }
}
