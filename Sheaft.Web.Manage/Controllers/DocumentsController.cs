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
using Sheaft.Application.Queries;

namespace Sheaft.Manage.Controllers
{
    public class DocumentsController : ManageController
    {
        private readonly ILogger<DocumentsController> _logger;
        private readonly IDocumentQueries _documentQueries;

        public DocumentsController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IDocumentQueries documentQueries,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider,
            ILogger<DocumentsController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
            _logger = logger;
            _documentQueries = documentQueries;
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

            return RedirectToAction("Edit", new { id = model.Id });
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

            byte[] data = null;
            using (var ms = new MemoryStream())
            {
                page.CopyTo(ms);
                ms.Position = 0;

                data = ms.ToArray();
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
        public async Task<IActionResult> Lock(Guid id, CancellationToken token)
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
        public async Task<IActionResult> Unlock(Guid id, CancellationToken token)
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
        public async Task<IActionResult> Validate(Guid id, CancellationToken token)
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
        public async Task<IActionResult> Download(Guid id, CancellationToken token)
        {
            var entity = await _context.Documents
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<DocumentViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            var data = await _documentQueries.DownloadDocumentAsync(id, await GetRequestUser(token), token);
            return File(data, "application/octet-stream", entity.Name + ".zip");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DownloadPage(Guid documentId, Guid pageId, CancellationToken token)
        {
            var entity = await _context.Documents
                .AsNoTracking()
                .Where(c => c.Id == documentId)
                .ProjectTo<DocumentViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            var page = entity.Pages.Single(p => p.Id == pageId);
            var data = await _documentQueries.DownloadDocumentPageAsync(documentId, pageId, await GetRequestUser(token), token);

            return File(data, "application/octet-stream", page.FileName + page.Extension);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePage(Guid documentId, Guid pageId, CancellationToken token)
        {
            var result = await _mediatr.Process(new DeletePageCommand(await GetRequestUser(token))
            {
                DocumentId = documentId,
                PageId = pageId
            }, token);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Exception.Message);
                return View("Edit", new { id = documentId });
            }

            TempData["Edited"] = JsonConvert.SerializeObject(new EntityViewModel { Id = pageId, Name = pageId.ToString("N") });
            return RedirectToAction("Edit", new { id = documentId });
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
