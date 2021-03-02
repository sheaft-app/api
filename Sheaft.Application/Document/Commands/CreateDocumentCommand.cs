﻿using System;
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
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var legal = await _context.GetSingleAsync<Domain.Legal>(r => r.Id == request.LegalId, token);
                if (legal.Documents.Any(d => d.Kind == request.Kind))
                    return Failure<Guid>(MessageKind.Document_CannotCreate_Type_Already_Present);

                var document = legal.AddDocument(request.Kind, request.Name);
                await _context.SaveChangesAsync(token);

                var result = await _pspService.CreateDocumentAsync(document, legal.User.Identifier, token);
                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync(token);
                    return Failure<Guid>(result.Exception);
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