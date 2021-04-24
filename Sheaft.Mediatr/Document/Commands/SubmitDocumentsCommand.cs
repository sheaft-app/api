using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Document.Commands
{
    public class SubmitDocumentsCommand : Command
    {
        protected SubmitDocumentsCommand()
        {
        }

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
            var legal = await _context.Legals.SingleAsync(e => e.Id == request.LegalId, token);
            Result result = null;
            foreach (var document in legal.Documents.Where(d => d.Status == DocumentStatus.Locked))
            {
                result = await _mediatr.Process(
                    new SubmitDocumentCommand(request.RequestUser) {DocumentId = document.Id}, token);
                if (!result.Succeeded)
                    break;
            }

            if (result is {Succeeded: false})
                return result;

            return Success();
        }
    }
}