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

namespace Sheaft.Mediatr.Document.Commands
{
    public class DeleteDocumentCommand : Command
    {
        protected DeleteDocumentCommand()
        {
            
        }
        [JsonConstructor]
        public DeleteDocumentCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DocumentId { get; set; }
    }

    public class DeleteDocumentCommandHandler : CommandsHandler,
        IRequestHandler<DeleteDocumentCommand, Result>
    {
        public DeleteDocumentCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<DeleteDocumentCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteDocumentCommand request, CancellationToken token)
        {
            var legal = await _context.Legals
                .SingleOrDefaultAsync(r => r.Documents.Any(d => d.Id == request.DocumentId), token);
            legal.DeleteDocument(request.DocumentId);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}