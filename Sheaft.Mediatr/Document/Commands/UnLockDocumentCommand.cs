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
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Document.Commands
{
    public class UnLockDocumentCommand : Command
    {
        protected UnLockDocumentCommand()
        {
            
        }
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
            var legal = await _context.Legals
                .SingleOrDefaultAsync(r => r.Documents.Any(d => d.Id == request.DocumentId), token);
            var document = legal.Documents.FirstOrDefault(c => c.Id == request.DocumentId);
            document.SetStatus(DocumentStatus.UnLocked);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}