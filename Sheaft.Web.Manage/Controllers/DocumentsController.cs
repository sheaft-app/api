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
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Queries;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models.ViewModels;
using Sheaft.Application.Common.Options;
using Sheaft.Application.Document.Commands;
using Sheaft.Application.Page.Commands;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Web.Manage.Controllers
{
    public class DocumentsController : ManageController
    {
        private readonly IDocumentQueries _documentQueries;

        public DocumentsController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IDocumentQueries documentQueries,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider)
            : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
            _documentQueries = documentQueries;
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid legalId, CancellationToken token)
        {
            var entity = await _context.Legals
                .Where(c => c.Id == legalId)
                .ProjectTo<LegalViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw SheaftException.NotFound();

            ViewBag.Kind = entity.Kind;
            ViewBag.UserId = entity.Owner.Id;
            ViewBag.LegalId = entity.Id;

            return View(new DocumentViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid legalId, DocumentViewModel model, CancellationToken token)
        {
            var result = await _mediatr.Process(new CreateDocumentCommand(await GetRequestUser(token))
            {
                Name = model.Name,
                Kind = model.Kind,
                LegalId = legalId
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id = result.Data});
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var entity = await _context.Legals
                .Where(c => c.Documents.Any(d => d.Id == id))
                .ProjectTo<LegalViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw SheaftException.NotFound();

            ViewBag.Kind = entity.Kind;
            ViewBag.UserId = entity.Owner.Id;
            ViewBag.LegalId = entity.Id;

            var document = entity.Documents.FirstOrDefault(d => d.Id == id);
            return View(document);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DocumentViewModel model, CancellationToken token)
        {
            var result = await _mediatr.Process(new UpdateDocumentCommand(await GetRequestUser(token))
            {
                DocumentId = model.Id,
                Name = model.Name,
                Kind = model.Kind,
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {model.Id});
        }

        [HttpGet]
        public async Task<IActionResult> AddPage(Guid documentId, CancellationToken token)
        {
            var entity = await _context.Legals
                .Where(c => c.Documents.Any(d => d.Id == documentId))
                .ProjectTo<LegalViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw SheaftException.NotFound();

            ViewBag.Kind = entity.Kind;
            ViewBag.UserId = entity.Owner.Id;

            var document = entity.Documents.FirstOrDefault(d => d.Id == documentId);
            return View(document);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPage(DocumentViewModel model, IFormFile page, CancellationToken token)
        {
            if (page == null)
                throw new ArgumentNullException("page");

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

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {model.Id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Lock(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new LockDocumentCommand(await GetRequestUser(token))
            {
                DocumentId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unlock(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new UnLockDocumentCommand(await GetRequestUser(token))
            {
                DocumentId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Validate(Guid id, CancellationToken token)
        {
            var result = await _mediatr.Process(new SubmitDocumentCommand(await GetRequestUser(token))
            {
                DocumentId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Download(Guid id, CancellationToken token)
        {
            var entity = await _context.Legals
                .SelectMany(c => c.Documents)
                .ProjectTo<DocumentViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(c => c.Id == id, token);

            if (entity == null)
                throw SheaftException.NotFound();

            var data = await _documentQueries.DownloadDocumentAsync(id, await GetRequestUser(token), token);
            return File(data, "application/octet-stream", entity.Name + ".zip");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DownloadPage(Guid documentId, Guid pageId, CancellationToken token)
        {
            var entity = await _context.Legals
                .SelectMany(c => c.Documents)
                .ProjectTo<DocumentViewModel>(_configurationProvider)
                .SingleOrDefaultAsync(c => c.Id == documentId && c.Pages.Any(p => p.Id == pageId), token);

            if (entity == null)
                throw SheaftException.NotFound();

            var page = entity.Pages.Single(p => p.Id == pageId);
            var data = await _documentQueries.DownloadDocumentPageAsync(documentId, pageId, await GetRequestUser(token),
                token);

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

            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id = documentId});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token)
        {
            var entity = await _context.Legals
                .SingleOrDefaultAsync(c => c.Documents.Any(d => d.Id == id), token);

            var result = await _mediatr.Process(new DeleteDocumentCommand(await GetRequestUser(token))
            {
                DocumentId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            if (entity.Kind != LegalKind.Natural)
                return RedirectToAction("EditLegalBusiness", "Legals", new {entity.Id});

            return RedirectToAction("EditLegalConsumer", "Legals", new {entity.Id});
        }
    }
}