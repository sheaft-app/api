using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Document.Commands
{
    public class UpdateDocumentCommand : Command
    {
        [JsonConstructor]
        public UpdateDocumentCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DocumentId { get; set; }
        public string Name { get; set; }
        public DocumentKind Kind { get; set; }
    }

    public class UpdateDocumentCommandHandler : CommandsHandler,
        IRequestHandler<UpdateDocumentCommand, Result>
    {
        private readonly IPspService _pspService;

        public UpdateDocumentCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            ILogger<UpdateDocumentCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result> Handle(UpdateDocumentCommand request, CancellationToken token)
        {
            var legal = await _context.GetSingleAsync<Domain.Legal>(r => r.Documents.Any(d => d.Id == request.DocumentId), token);
            var document = legal.Documents.FirstOrDefault(c => c.Id == request.DocumentId);
            if (document.Kind != request.Kind && legal.Documents.Any(d => d.Kind == request.Kind))
                return Failure(MessageKind.Document_CannotUpdate_Another_Document_With_Type_Exists);

            document.SetKind(request.Kind);
            document.SetName(request.Name);

            if (string.IsNullOrWhiteSpace(document.Identifier))
            {
                var result = await _pspService.CreateDocumentAsync(document, legal.User.Identifier, token);
                if (!result.Succeeded)
                    return Failure(result.Exception);

                document.SetIdentifier(result.Data.Identifier);
                document.SetStatus(result.Data.Status);
            }

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}