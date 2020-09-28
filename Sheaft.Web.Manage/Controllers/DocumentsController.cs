using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Commands;
using Sheaft.Exceptions;
using Sheaft.Application.Interop;
using Sheaft.Manage.Models;
using Sheaft.Application.Models;
using Sheaft.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Manage.Controllers
{
    public class DocumentsController : ManageController
    {
        private readonly ILogger<DocumentsController> _logger;

        public DocumentsController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider,
            ILogger<DocumentsController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var entity = await _context.Documents
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<DocumentViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DocumentViewModel model, CancellationToken token)
        {
            var result = await _mediatr.Process(new UpdateDocumentCommand(await GetRequestUser(token))
            {
                Id = model.Id,
                Name = model.Name,
                Kind = model.Kind,
            }, token);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid legalId, CancellationToken token)
        {
            var entity = await _context.Legals
                .AsNoTracking()
                .Where(c => c.Id == legalId)
                .ProjectTo<LegalViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            return View(new DocumentViewModel { Legal = new LegalViewModel { Id = legalId } });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DocumentViewModel model, CancellationToken token)
        {
            var result = await _mediatr.Process(new CreateDocumentCommand(await GetRequestUser(token))
            {
                Name = model.Name,
                Kind = model.Kind,
                LegalId = model.Legal.Id
            }, token);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            return RedirectToAction("Edit", new { id = result.Data });
        }

        [HttpGet]
        public async Task<IActionResult> AddPage(Guid documentId, CancellationToken token)
        {
            var entity = await _context.Documents
                .AsNoTracking()
                .Where(c => c.Id == documentId)
                .ProjectTo<DocumentViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPage(DocumentViewModel model, IFormFile page, CancellationToken token)
        {
            if (page == null)
            {
                ModelState.AddModelError("", "Page document is required.");
                return View(model);
            }

            var data = string.Empty;
            using (var ms = new MemoryStream())
            {
                page.CopyTo(ms);
                data = Convert.ToBase64String(ms.ToArray());
            }

            var result = await _mediatr.Process(new UploadPageCommand(await GetRequestUser(token))
            {
                DocumentId = model.Id,
                Extension = Path.GetExtension(page.FileName),
                FileName = Path.GetFileNameWithoutExtension(page.FileName),
                Size = page.Length,
                Data = data
            }, token);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return View(model);
            }

            TempData["Edited"] = JsonConvert.SerializeObject(new EntityViewModel { Id = model.Id, Name = model.Name });
            return RedirectToAction("Edit", new { id = model.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DownloadDocument(Guid id, CancellationToken token)
        {
            var entity = await _context.Documents
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<DocumentViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            return View("Edit", new { id = entity.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DownloadPage(Guid id, CancellationToken token)
        {
            var entity = await _context.Documents
                .AsNoTracking()
                .Where(c => c.Pages.Any(p => p.Id == id))
                .ProjectTo<DocumentViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            return View("Edit", new { id = entity.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token)
        {
            var entity = await _context.Documents.SingleOrDefaultAsync(c => c.Id == id, token);
            var name = entity.Name;

            var requestUser = await GetRequestUser(token);
            var result = await _mediatr.Process(new DeleteDocumentCommand(requestUser)
            {
                Id = id
            }, token);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return RedirectToAction("Edit", new { id });
            }

            return RedirectToAction("UpdateLegal", "Producers", new { id = entity.Legal.Id });
        }
    }
}
