﻿using System;
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
    public class LockDocumentsCommand : Command
    {
        protected LockDocumentsCommand()
        {
        }

        [JsonConstructor]
        public LockDocumentsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid LegalId { get; set; }
    }

    public class LockDocumentsCommandHandler : CommandsHandler,
        IRequestHandler<LockDocumentsCommand, Result>
    {
        public LockDocumentsCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<LockDocumentsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(LockDocumentsCommand request, CancellationToken token)
        {
            var legal = await _context.Legals.SingleAsync(e => e.Id == request.LegalId, token);

            Result result = null;
            foreach (var document in legal.Documents.Where(d =>
                d.Status == DocumentStatus.UnLocked || d.Status == DocumentStatus.Created))
            {
                result = await _mediatr.Process(new LockDocumentCommand(request.RequestUser) {DocumentId = document.Id},
                    token);
                if (!result.Succeeded)
                    break;
            }

            if (result is {Succeeded: false})
                return result;

            return Success();
        }
    }
}