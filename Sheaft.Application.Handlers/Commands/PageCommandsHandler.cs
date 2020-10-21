using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Commands;
using Sheaft.Core;
using Sheaft.Application.Interop;
using Microsoft.Extensions.Logging;
using Sheaft.Domain.Models;
using System.Linq;

namespace Sheaft.Application.Handlers
{
    public class PageCommandsHandler : ResultsHandler,
            IRequestHandler<UploadPageCommand, Result<Guid>>,
            IRequestHandler<SendPageCommand, Result<bool>>,
            IRequestHandler<DeletePageCommand, Result<bool>>
    {
        private readonly IPspService _pspService;
        private readonly IBlobService _blobService;

        public PageCommandsHandler(
            IAppDbContext context,
            IPspService pspService,
            IBlobService blobService,
            ISheaftMediatr mediatr,
            ILogger<PageCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _blobService = blobService;
        }

        public async Task<Result<Guid>> Handle(UploadPageCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var page = new Page(Guid.NewGuid(), request.FileName, request.Extension, request.Size);

                var legal = await _context.GetSingleAsync<Legal>(r => r.Documents.Any(d => d.Id == request.DocumentId), token);
                var document = legal.Documents.FirstOrDefault(d => d.Id == request.DocumentId);
                document.AddPage(page);

                await _context.SaveChangesAsync(token);

                var result = await _blobService.UploadDocumentPageAsync(document.Id, page.Id, request.Data, legal.User.Id, token);
                if (!result.Success)
                    return Failed<Guid>(result.Exception);

                return Ok(page.Id);
            });
        }

        public async Task<Result<bool>> Handle(SendPageCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var legal = await _context.GetSingleAsync<Legal>(r => r.Documents.Any(d => d.Id == request.DocumentId), token);
                var document = legal.Documents.FirstOrDefault(d => d.Id == request.DocumentId);

                var page = document.Pages.SingleOrDefault(p => p.Id == request.PageId);

                var downloadResult = await _blobService.DownloadDocumentPageAsync(document.Id, page.Id, legal.User.Id, token);
                if (!downloadResult.Success)
                    return Failed<bool>(downloadResult.Exception);

                var result = await _pspService.AddPageToDocumentAsync(page, document, legal.User.Identifier, downloadResult.Data, token);
                if (!result.Success)
                    return result;

                page.SetUploaded();

                await _context.SaveChangesAsync(token);
                return result;
            });
        }

        public async Task<Result<bool>> Handle(DeletePageCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var legal = await _context.GetSingleAsync<Legal>(r => r.Documents.Any(d => d.Id == request.DocumentId), token);
                var document = legal.Documents.FirstOrDefault(d => d.Id == request.DocumentId);

                var deleteResult = await _blobService.DeleteDocumentPageAsync(document.Id, request.PageId, legal.User.Id, token);
                if (!deleteResult.Success)
                    return Failed<bool>(deleteResult.Exception);

                document.DeletePage(request.PageId);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}
