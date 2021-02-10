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

namespace Sheaft.Application.Document.Commands
{
    public class DeleteDocumentCommand : Command
    {
        [JsonConstructor]
        public DeleteDocumentCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }

    public class DeleteDocumentCommandHandler : CommandsHandler,
        IRequestHandler<DeleteDocumentCommand, Result>
    {
        private readonly IPspService _pspService;

        public DeleteDocumentCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            ILogger<DeleteDocumentCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result> Handle(DeleteDocumentCommand request, CancellationToken token)
        {
            var legal = await _context.GetSingleAsync<Domain.Legal>(r => r.Documents.Any(d => d.Id == request.Id), token);
            legal.DeleteDocument(request.Id);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}