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
            return await ExecuteAsync(request, async () =>
            {
                var page = new Page(Guid.NewGuid(), request.FileName, request.Extension, request.Size);

                var legal = await _context.GetSingleAsync<Legal>(r => r.Documents.Any(d => d.Id == request.DocumentId),
                    token);
                var document = legal.Documents.FirstOrDefault(d => d.Id == request.DocumentId);
                document.AddPage(page);

                await _context.SaveChangesAsync(token);

                var result =
                    await _blobService.UploadDocumentPageAsync(document.Id, page.Id, request.Data, legal.User.Id,
                        token);
                if (!result.Success)
                    return Failed<Guid>(result.Exception);

                return Ok(page.Id);
            });
        }
    }
}
