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
    public class UpdateDocumentCommand : Command
    {
        protected UpdateDocumentCommand()
        {
            
        }
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
            var legal = await _context.Legals
                .SingleOrDefaultAsync(r => r.Documents.Any(d => d.Id == request.DocumentId), token);
            var document = legal.Documents.FirstOrDefault(c => c.Id == request.DocumentId);
            if (document.Kind != request.Kind && legal.Documents.Any(d => d.Kind == request.Kind))
                return Failure("Impossible de mettre à jour le type du document, un document avec ce nouveau type existe déjà.");

            document.SetKind(request.Kind);
            document.SetName(request.Name);

            if (string.IsNullOrWhiteSpace(document.Identifier))
            {
                var result = await _pspService.CreateDocumentAsync(document, legal.User.Identifier, token);
                if (!result.Succeeded)
                    return Failure(result);

                document.SetIdentifier(result.Data.Identifier);
                document.SetStatus(result.Data.Status);
            }

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}