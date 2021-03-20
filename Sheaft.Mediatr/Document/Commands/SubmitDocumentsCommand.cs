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

namespace Sheaft.Mediatr.Document.Commands
{
    public class SubmitDocumentsCommand : Command
    {
        [JsonConstructor]
        public SubmitDocumentsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid LegalId { get; set; }
    }

    public class SubmitDocumentsCommandHandler : CommandsHandler,
        IRequestHandler<SubmitDocumentsCommand, Result>
    {
        public SubmitDocumentsCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<SubmitDocumentsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(SubmitDocumentsCommand request, CancellationToken token)
        {
            var legal = await _context.GetSingleAsync<Domain.Legal>(r => r.Id == request.LegalId, token);
            var success = true;
            foreach (var document in legal.Documents.Where(d => d.Status == DocumentStatus.Locked))
            {
                var result = await _mediatr.Process(new SubmitDocumentCommand(request.RequestUser)
                {
                    DocumentId = document.Id
                }, token);

                if (!result.Succeeded)
                    success = false;
            }

            return success ? Success() : Failure();
        }
    }
}