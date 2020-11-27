using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Commands;
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
    public class TagsController : ManageController
    {
        private readonly ILogger<TagsController> _logger;

        public TagsController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider,
            ILogger<TagsController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
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
        public async Task<IActionResult> Add(TagViewModel model, IFormFile picture, IFormFile icon, CancellationToken token)
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

            var result = await _mediatr.Process(new CreateTagCommand(await GetRequestUser(token))
            {
                Description = model.Description,
                Name = model.Name,
                Picture = model.Picture,
                Icon = model.Icon,
                Available = model.Available,
                Kind = model.Kind
            }, token);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("Edit", new { id = result.Data });
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
                throw new NotFoundException();

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TagViewModel model, IFormFile picture, IFormFile icon, CancellationToken token)
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

            var result = await _mediatr.Process(new UpdateTagCommand(await GetRequestUser(token))
            {
                Id = model.Id,
                Description = model.Description,
                Name = model.Name,
                Picture = model.Picture,
                Icon = model.Icon,
                Available = model.Available,
                Kind = model.Kind
            }, token);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("Edit", new { model.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new RestoreTagCommand(await GetRequestUser(token))
            {
                Id = id
            }, token);

            if (!result.Success)
                throw result.Exception;

            return RedirectToAction("Edit", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new DeleteTagCommand(await GetRequestUser(token))
            {
                Id = id
            }, token);

            if (!result.Success)
                throw result.Exception;

            return RedirectToAction("Index");
        }
    }
}
