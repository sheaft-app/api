using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Document.Commands
{
    public class CreateDocumentCommand : Command<Guid>
    {
        protected CreateDocumentCommand()
        {
            
        }
        [JsonConstructor]
        public CreateDocumentCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid LegalId { get; set; }
        public string Name { get; set; }
        public DocumentKind Kind { get; set; }
    }

    public class CreateDocumentCommandHandler : CommandsHandler,
        IRequestHandler<CreateDocumentCommand, Result<Guid>>
    {
        private readonly IPspService _pspService;

        public CreateDocumentCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            ILogger<CreateDocumentCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<Guid>> Handle(CreateDocumentCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var legal = await _context.Legals.SingleAsync(e => e.Id == request.LegalId, token);
                if (legal.Documents.Any(d => d.Kind == request.Kind))
                    return Failure<Guid>("Impossible de créer ce document, un document de ce type existe déjà.");

                var document = legal.AddDocument(request.Kind, request.Name);
                await _context.SaveChangesAsync(token);

                var result = await _pspService.CreateDocumentAsync(document, legal.User.Identifier, token);
                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync(token);
                    return Failure<Guid>(result);
                }

                document.SetIdentifier(result.Data.Identifier);
                document.SetStatus(result.Data.Status);

                await _context.SaveChangesAsync(token);
                await transaction.CommitAsync(token);

                return Success(document.Id);
            }
        }
    }
}