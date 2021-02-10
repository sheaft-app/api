using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Page.Commands
{
    public class UploadPageCommand : Command<Guid>
    {
        [JsonConstructor]
        public UploadPageCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DocumentId { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
        public byte[] Data { get; set; }
    }

    public class UploadPageCommandHandler : CommandsHandler,
        IRequestHandler<UploadPageCommand, Result<Guid>>
    {
        private readonly IPspService _pspService;
        private readonly IBlobService _blobService;

        public UploadPageCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            IBlobService blobService,
            ISheaftMediatr mediatr,
            ILogger<UploadPageCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _blobService = blobService;
        }

        public async Task<Result<Guid>> Handle(UploadPageCommand request, CancellationToken token)
        {
            var page = new Domain.Page(Guid.NewGuid(), request.FileName, request.Extension, request.Size);

            var legal = await _context.GetSingleAsync<Domain.Legal>(r => r.Documents.Any(d => d.Id == request.DocumentId),
                token);
            var document = legal.Documents.FirstOrDefault(d => d.Id == request.DocumentId);
            document.AddPage(page);

            await _context.SaveChangesAsync(token);

            var result =
                await _blobService.UploadDocumentPageAsync(document.Id, page.Id, request.Data, legal.User.Id,
                    token);
            if (!result.Succeeded)
                return Failure<Guid>(result.Exception);

            return Success(page.Id);
        }
    }
}