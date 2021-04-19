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
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Document.Commands
{
    public class LockDocumentCommand : Command
    {
        protected LockDocumentCommand()
        {
            
        }
        [JsonConstructor]
        public LockDocumentCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DocumentId { get; set; }
    }

    public class LockDocumentCommandHandler : CommandsHandler,
        IRequestHandler<LockDocumentCommand, Result>
    {
        public LockDocumentCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<LockDocumentCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(LockDocumentCommand request, CancellationToken token)
        {
            var legal = await _context.Legals
                .SingleOrDefaultAsync(r => r.Documents.Any(d => d.Id == request.DocumentId), token);
            var document = legal.Documents.SingleOrDefault(c => c.Id == request.DocumentId);
            document.SetStatus(DocumentStatus.Locked);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}