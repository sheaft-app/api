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
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Document.Commands
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
        private readonly IPspService _pspService;

        public UnLockDocumentCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            ILogger<UnLockDocumentCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result> Handle(UnLockDocumentCommand request, CancellationToken token)
        {
            var legal = await _context.GetSingleAsync<Domain.Legal>(r => r.Documents.Any(d => d.Id == request.DocumentId),
                token);
            var document = legal.Documents.FirstOrDefault(c => c.Id == request.DocumentId);
            document.SetStatus(DocumentStatus.UnLocked);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}