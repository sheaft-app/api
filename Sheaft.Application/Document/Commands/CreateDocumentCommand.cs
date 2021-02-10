using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Exceptions;

namespace Sheaft.Application.Commands
{
    public class CreateDocumentCommand : Command<Guid>
    {
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
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    var legal = await _context.GetSingleAsync<Legal>(r => r.Id == request.LegalId, token);
                    if (legal.Documents.Any(d => d.Kind == request.Kind))
                        return BadRequest<Guid>(MessageKind.Document_CannotCreate_Type_Already_Present);

                    var document = legal.AddDocument(request.Kind, request.Name);
                    await _context.SaveChangesAsync(token);

                    var result = await _pspService.CreateDocumentAsync(document, legal.User.Identifier, token);
                    if (!result.Success)
                    {
                        await transaction.RollbackAsync(token);
                        return Failed<Guid>(result.Exception);
                    }

                    document.SetIdentifier(result.Data.Identifier);
                    document.SetStatus(result.Data.Status);

                    await _context.SaveChangesAsync(token);
                    await transaction.CommitAsync(token);

                    return Ok(document.Id);
                }
            });
        }
    }
}
