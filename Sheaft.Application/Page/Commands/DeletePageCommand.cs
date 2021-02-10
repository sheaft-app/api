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
    public class DeletePageCommand : Command<bool>
    {
        [JsonConstructor]
        public DeletePageCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DocumentId { get; set; }
        public Guid PageId { get; set; }
    }

    public class DeletePageCommandHandler : CommandsHandler,
        IRequestHandler<DeletePageCommand, Result<bool>>
    {
        private readonly IPspService _pspService;
        private readonly IBlobService _blobService;

        public DeletePageCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            IBlobService blobService,
            ISheaftMediatr mediatr,
            ILogger<DeletePageCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _blobService = blobService;
        }
        
        public async Task<Result<bool>> Handle(DeletePageCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var legal = await _context.GetSingleAsync<Legal>(r => r.Documents.Any(d => d.Id == request.DocumentId),
                    token);
                var document = legal.Documents.FirstOrDefault(d => d.Id == request.DocumentId);

                var deleteResult =
                    await _blobService.DeleteDocumentPageAsync(document.Id, request.PageId, legal.User.Id, token);
                if (!deleteResult.Success)
                    return Failed<bool>(deleteResult.Exception);

                document.DeletePage(request.PageId);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}
