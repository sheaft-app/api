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
    public class DeletePageCommand : Command
    {
        [JsonConstructor]
        public DeletePageCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DocumentId { get; set; }
        public Guid PageId { get; set; }
    }

    public class DeletePageCommandHandler : CommandsHandler,
        IRequestHandler<DeletePageCommand, Result>
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

        public async Task<Result> Handle(DeletePageCommand request, CancellationToken token)
        {
            var legal = await _context.GetSingleAsync<Domain.Legal>(r => r.Documents.Any(d => d.Id == request.DocumentId),
                token);
            var document = legal.Documents.FirstOrDefault(d => d.Id == request.DocumentId);

            var deleteResult =
                await _blobService.DeleteDocumentPageAsync(document.Id, request.PageId, legal.User.Id, token);
            if (!deleteResult.Succeeded)
                return Failure(deleteResult.Exception);

            document.DeletePage(request.PageId);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}