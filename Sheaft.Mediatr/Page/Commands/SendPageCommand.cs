using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Page.Commands
{
    public class SendPageCommand : Command
    {
        protected SendPageCommand()
        {
            
        }
        [JsonConstructor]
        public SendPageCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DocumentId { get; set; }
        public Guid PageId { get; set; }
    }

    public class SendPageCommandHandler : CommandsHandler,
        IRequestHandler<SendPageCommand, Result>
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

        public async Task<Result> Handle(SendPageCommand request, CancellationToken token)
        {
            var legal = await _context.Legals
                .SingleOrDefaultAsync(r => r.Documents.Any(d => d.Id == request.DocumentId), token);
            var document = legal.Documents.FirstOrDefault(d => d.Id == request.DocumentId);

            var page = document.Pages.SingleOrDefault(p => p.Id == request.PageId);

            var downloadResult =
                await _blobService.DownloadDocumentPageAsync(document.Id, page.Id, legal.User.Id, token);
            if (!downloadResult.Succeeded)
                return Failure(downloadResult);

            var result = await _pspService.AddPageToDocumentAsync(page, document, legal.User.Identifier,
                downloadResult.Data, token);
            if (!result.Succeeded)
                return result;

            page.SetUploaded();

            await _context.SaveChangesAsync(token);
            return result;
        }
    }
}