using System;
using Sheaft.Core;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class SendPageCommand : Command<bool>
    {
        [JsonConstructor]
        public SendPageCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DocumentId { get; set; }
        public Guid PageId { get; set; }
    }

    public class SendPageCommandHandler : CommandsHandler,
        IRequestHandler<SendPageCommand, Result<bool>>
    {
        private readonly IPspService _pspService;
        private readonly IBlobService _blobService;

        public SendPageCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            IBlobService blobService,
            ISheaftMediatr mediatr,
            ILogger<SendPageCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _blobService = blobService;
        }
        
        public async Task<Result<bool>> Handle(SendPageCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var legal = await _context.GetSingleAsync<Legal>(r => r.Documents.Any(d => d.Id == request.DocumentId),
                    token);
                var document = legal.Documents.FirstOrDefault(d => d.Id == request.DocumentId);

                var page = document.Pages.SingleOrDefault(p => p.Id == request.PageId);

                var downloadResult =
                    await _blobService.DownloadDocumentPageAsync(document.Id, page.Id, legal.User.Id, token);
                if (!downloadResult.Success)
                    return Failed<bool>(downloadResult.Exception);

                var result = await _pspService.AddPageToDocumentAsync(page, document, legal.User.Identifier,
                    downloadResult.Data, token);
                if (!result.Success)
                    return result;

                page.SetUploaded();

                await _context.SaveChangesAsync(token);
                return result;
            });
        }
    }
}
