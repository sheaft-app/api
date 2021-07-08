using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
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
using Sheaft.Mediatr.Business.Commands;
using Sheaft.Mediatr.Producer.Commands;
using Sheaft.Mediatr.User.Commands;
using Sheaft.Options;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Controllers
{
    public class ProducersController : ManageController
    {
        public ProducersController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IConfigurationProvider configurationProvider,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken token, int page = 0, int take = 50)
        {
            if (page < 0)
                page = 0;

            if (take > 100)
                take = 100;

            var query = _context.Users.OfType<Producer>().AsNoTracking();

            var requestUser = await GetRequestUserAsync(token);
            if (requestUser.IsImpersonating())
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
                throw SheaftException.NotFound();

            ViewBag.LegalsId = (await _context.Legals.FirstOrDefaultAsync(c => c.UserId == id, token))?.Id;
            ViewBag.BankAccountId = (await _context.BankAccounts.FirstOrDefaultAsync(c => c.UserId == id, token))?.Id;

            ViewBag.Tags = await GetTags(token);
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProducerViewModel model, List<IFormFile> newPictures,
            CancellationToken token)
        {
            var images =
                model.Pictures?.Where(p => !p.Remove).Select(p => new PictureInputDto
                    {Id = p.Id, Data = p.Url, Position = p.Position})?.ToList() ?? new List<PictureInputDto>();
            if (newPictures != null && newPictures.Any())
            {
                foreach (var picture in newPictures)
                {
                    using (var ms = new MemoryStream())
                    {
                        await picture.CopyToAsync(ms, token);
                        images.Add(new PictureInputDto
                            {Data = Convert.ToBase64String(ms.ToArray()), Position = images.Count});
                    }
                }
            }

            var producer = await _context.Users.OfType<Producer>().SingleOrDefaultAsync(c => c.Id == model.Id, token);
            var requestUser = new RequestUser() {Id = producer.Id};
            if (model.Closings != null)
            {
                var removeClosingsResult = await _mediatr.Process(
                    new DeleteBusinessClosingsCommand(requestUser)
                    {
                        ClosingIds = model.Closings?
                            .Where(c => c.Id.HasValue && c.Remove)
                            .Select(c => c.Id.Value).ToList()
                    }, token);
                
                if (!removeClosingsResult.Succeeded)
                    throw removeClosingsResult.Exception;
                
                var newClosingsResult = await _mediatr.Process(
                    new UpdateOrCreateBusinessClosingsCommand(requestUser)
                    {
                        UserId = producer.Id, Closings = model.Closings?
                            .Where(c => !c.Remove && c.ClosedFrom.HasValue && c.ClosedTo.HasValue)
                            .Select(c => new ClosingInputDto
                            {
                                Id = c.Id,
                                From = c.ClosedFrom.Value,
                                To = c.ClosedTo.Value,
                                Reason = c.Reason
                            }).ToList()
                    }, token);

                if (!newClosingsResult.Succeeded)
                    throw newClosingsResult.Exception;
            }

            var result = await _mediatr.Process(new UpdateProducerCommand(requestUser)
            {
                ProducerId = model.Id,
                Address = _mapper.Map<AddressDto>(model.Address),
                OpenForNewBusiness = model.OpenForNewBusiness,
                Email = model.Email,
                Name = model.Name,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Kind = model.Kind,
                Phone = model.Phone,
                Tags = producer.Tags.Select(t => t.Tag.Id),
                NotSubjectToVat = model.NotSubjectToVat,
                Pictures = images,
            }, token);

            if (!result.Succeeded)
            {
                ViewBag.LegalsId = (await _context.Legals.FirstOrDefaultAsync(c => c.UserId == model.Id, token))?.Id;
                ViewBag.BankAccountId =
                    (await _context.BankAccounts.FirstOrDefaultAsync(c => c.UserId == model.Id, token))?.Id;
                ViewBag.Tags = await GetTags(token);

                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("Edit", new {model.Id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new DeleteUserCommand(await GetRequestUserAsync(token))
            {
                UserId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new RestoreUserCommand(await GetRequestUserAsync(token))
            {
                UserId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id});
        }
    }
}