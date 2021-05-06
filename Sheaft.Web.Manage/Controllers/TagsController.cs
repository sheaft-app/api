using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core.Exceptions;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Mediatr.Tag.Commands;
using Sheaft.Options;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Controllers
{
    public class TagsController : ManageController
    {
        public TagsController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider)
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

            var query = _context.Tags.AsNoTracking();

            var entities = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip(page * take)
                .Take(take)
                .ProjectTo<TagViewModel>(_configurationProvider)
                .ToListAsync(token);

            ViewBag.Page = page;
            ViewBag.Take = take;

            return View(entities);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new TagViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TagViewModel model, IFormFile picture, IFormFile icon,
            CancellationToken token)
        {
            if (picture != null)
            {
                using (var ms = new MemoryStream())
                {
                    picture.CopyTo(ms);
                    model.Picture = Convert.ToBase64String(ms.ToArray());
                }
            }

            if (icon != null)
            {
                using (var ms = new MemoryStream())
                {
                    icon.CopyTo(ms);
                    model.Icon = Convert.ToBase64String(ms.ToArray());
                }
            }

            var result = await _mediatr.Process(new CreateTagCommand(await GetRequestUserAsync(token))
            {
                Description = model.Description,
                Name = model.Name,
                Picture = model.Picture,
                Icon = model.Icon,
                Available = model.Available,
                Kind = model.Kind
            }, token);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var entity = await _context.Tags
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<TagViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw SheaftException.NotFound();

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TagViewModel model, IFormFile picture, IFormFile icon,
            CancellationToken token)
        {
            if (picture != null)
            {
                using (var ms = new MemoryStream())
                {
                    picture.CopyTo(ms);
                    model.Picture = Convert.ToBase64String(ms.ToArray());
                }
            }

            if (icon != null)
            {
                using (var ms = new MemoryStream())
                {
                    icon.CopyTo(ms);
                    model.Icon = Convert.ToBase64String(ms.ToArray());
                }
            }

            var result = await _mediatr.Process(new UpdateTagCommand(await GetRequestUserAsync(token))
            {
                TagId = model.Id,
                Description = model.Description,
                Name = model.Name,
                Picture = model.Picture,
                Icon = model.Icon,
                Available = model.Available,
                Kind = model.Kind
            }, token);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new RestoreTagCommand(await GetRequestUserAsync(token))
            {
                TagId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new DeleteTagCommand(await GetRequestUserAsync(token))
            {
                TagId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Index");
        }
    }
}