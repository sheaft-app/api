using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Services.Document.Commands
{
    public class UnLockDocumentCommand : Command
    {
        [JsonConstructor]
        public UnLockDocumentCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DocumentId { get; set; }
    }

    public class UnLockDocumentCommandHandler : CommandsHandler,
        IRequestHandler<UnLockDocumentCommand, Result>
    {
        public UnLockDocumentCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<UnLockDocumentCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UnLockDocumentCommand request, CancellationToken token)
        {
            var legal = await _context.GetSingleAsync<Domain.Legal>(
                r => r.Documents.Any(d => d.Id == request.DocumentId),
                token);
            var document = legal.Documents.FirstOrDefault(c => c.Id == request.DocumentId);
            document.SetStatus(DocumentStatus.UnLocked);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}