using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.Document.Commands;
using Sheaft.Mediatr.Page.Commands;
using Sheaft.Options;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Controllers
{
    public class DocumentsController : ManageController
    {
        private readonly IBlobService _blobService;

        public DocumentsController(
            IAppDbContext context,
            IMapper mapper,
            IBlobService blobService,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider)
            : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
            _blobService = blobService;
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
            ViewBag.UserId = entity.UserId;
            ViewBag.LegalId = entity.Id;

            return View(new DocumentViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid legalId, DocumentViewModel model, CancellationToken token)
        {
            var result = await _mediatr.Process(new CreateDocumentCommand(await GetRequestUserAsync(token))
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
            ViewBag.UserId = entity.UserId;
            ViewBag.LegalId = entity.Id;

            var document = entity.Documents.FirstOrDefault(d => d.Id == id);
            return View(document);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DocumentViewModel model, CancellationToken token)
        {
            var result = await _mediatr.Process(new UpdateDocumentCommand(await GetRequestUserAsync(token))
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
            ViewBag.UserId = entity.UserId;

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

            var result = await _mediatr.Process(new UploadPageCommand(await GetRequestUserAsync(token))
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
            var result = await _mediatr.Process(new LockDocumentCommand(await GetRequestUserAsync(token))
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
            var result = await _mediatr.Process(new UnLockDocumentCommand(await GetRequestUserAsync(token))
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
            var result = await _mediatr.Process(new SubmitDocumentCommand(await GetRequestUserAsync(token))
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

            var data = await DownloadDocumentAsync(id, await GetRequestUserAsync(token), token);
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
            var data = await DownloadDocumentPageAsync(documentId, pageId, await GetRequestUserAsync(token),
                token);

            return File(data, "application/octet-stream", page.FileName + page.Extension);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePage(Guid documentId, Guid pageId, CancellationToken token)
        {
            var result = await _mediatr.Process(new DeletePageCommand(await GetRequestUserAsync(token))
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

            var result = await _mediatr.Process(new DeleteDocumentCommand(await GetRequestUserAsync(token))
            {
                DocumentId = id
            }, token);

            if (!result.Succeeded)
                throw result.Exception;

            if (entity.Kind != LegalKind.Natural)
                return RedirectToAction("EditLegalBusiness", "Legals", new {entity.Id});

            return RedirectToAction("EditLegalConsumer", "Legals", new {entity.Id});
        }
        public async Task<byte[]> DownloadDocumentAsync(Guid documentId, RequestUser currentUser, CancellationToken token)
        {
            var legal = await _context.Legals
                    .SingleOrDefaultAsync(l => l.Documents.Any(d => d.Id == documentId), token);

            var document = legal.Documents.FirstOrDefault(d => d.Id == documentId);
            if (document == null)
                return null;

            byte[] archiveFile;
            using (var archiveStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
                {
                    foreach (var page in document.Pages)
                    {
                        var result = await _blobService.DownloadDocumentPageAsync(document.Id, page.Id, legal.User.Id, token);
                        if (!result.Succeeded)
                            throw SheaftException.Unexpected(result.Exception, result.Message, result.Params);

                        var zipArchiveEntry = archive.CreateEntry(page.Filename + page.Extension, CompressionLevel.Optimal);
                        using (var zipStream = zipArchiveEntry.Open())
                            await zipStream.WriteAsync(result.Data, 0, result.Data.Length, token);
                    }
                }

                archiveFile = archiveStream.ToArray();
            }

            return archiveFile;
        }

        public async Task<byte[]> DownloadDocumentPageAsync(Guid documentId, Guid pageId, RequestUser currentUser, CancellationToken token)
        {
            var legal = await _context.Legals
                    .SingleOrDefaultAsync(l => l.Documents.Any(d => d.Id == documentId), token);

            var document = legal.Documents.FirstOrDefault(d => d.Id == documentId);
            if (document == null)
                return null;

            var result = await _blobService.DownloadDocumentPageAsync(document.Id, pageId, legal.User.Id, token);
            if (!result.Succeeded)
                return null;

            return result.Data;
        }
    }
}