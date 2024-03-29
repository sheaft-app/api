﻿using System;
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

namespace Sheaft.Mediatr.Declaration.Commands
{
    public class SubmitDeclarationCommand : Command
    {
        protected SubmitDeclarationCommand()
        {
            
        }
        [JsonConstructor]
        public SubmitDeclarationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeclarationId { get; set; }
    }

    public class SubmitDeclarationCommandHandler : CommandsHandler,
        IRequestHandler<SubmitDeclarationCommand, Result>
    {
        private readonly IPspService _pspService;

        public SubmitDeclarationCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<SubmitDeclarationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result> Handle(SubmitDeclarationCommand request, CancellationToken token)
        {
            var legal = await _context.Set<BusinessLegal>()
                .SingleOrDefaultAsync(r => r.DeclarationId == request.DeclarationId, token);
            if (legal.Declaration.Status != DeclarationStatus.Locked)
                return Failure("Impossible d'envoyer la déclaration, elle doit être vérouillée avant l'envoi.");

            if (string.IsNullOrWhiteSpace(legal.Declaration.Identifier))
            {
                var createResult =
                    await _mediatr.Process(new CreateDeclarationCommand(request.RequestUser) {LegalId = legal.Id},
                        token);
                if (!createResult.Succeeded)
                    return Failure(createResult);
            }

            var submitResult = await _pspService.SubmitUboDeclarationAsync(legal.Declaration, legal.User, token);
            if (!submitResult.Succeeded)
                return Failure(submitResult);

            legal.Declaration.SetStatus(submitResult.Data.Status);
            legal.Declaration.SetResult(submitResult.Data.ResultCode, submitResult.Data.ResultMessage);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}