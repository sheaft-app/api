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
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Page.Commands
{
    public class DeletePageCommand : Command
    {
        protected DeletePageCommand()
        {
            
        }
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
        private readonly IBlobService _blobService;

        public DeletePageCommandHandler(
            IAppDbContext context,
            IBlobService blobService,
            ISheaftMediatr mediatr,
            ILogger<DeletePageCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
        }

        public async Task<Result> Handle(DeletePageCommand request, CancellationToken token)
        {
            var legal = await _context.Legals
                .SingleOrDefaultAsync(r => r.Documents.Any(d => d.Id == request.DocumentId), token);
            var document = legal.Documents.FirstOrDefault(d => d.Id == request.DocumentId);

            var deleteResult =
                await _blobService.DeleteDocumentPageAsync(document.Id, request.PageId, legal.User.Id, token);
            if (!deleteResult.Succeeded)
                return Failure(deleteResult);

            document.DeletePage(request.PageId);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}