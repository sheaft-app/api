using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class DeleteDocumentCommand : Command<bool>
    {
        [JsonConstructor]
        public DeleteDocumentCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    
    public class DeleteDocumentCommandHandler : CommandsHandler,
            IRequestHandler<DeleteDocumentCommand, Result<bool>>
    {
        private readonly IPspService _pspService;

        public DeleteDocumentCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            ILogger<DeleteDocumentCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<bool>> Handle(DeleteDocumentCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var legal = await _context.GetSingleAsync<Legal>(r => r.Documents.Any(d => d.Id == request.Id), token);
                legal.DeleteDocument(request.Id);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}
