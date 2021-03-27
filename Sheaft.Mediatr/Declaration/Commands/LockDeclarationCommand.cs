﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Declaration.Commands
{
    public class LockDeclarationCommand : Command
    {
        [JsonConstructor]
        public LockDeclarationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeclarationId { get; set; }
    }

    public class LockDeclarationCommandHandler : CommandsHandler,
        IRequestHandler<LockDeclarationCommand, Result>
    {
        public LockDeclarationCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<LockDeclarationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(LockDeclarationCommand request, CancellationToken token)
        {
            var legal = await _context.GetSingleAsync<BusinessLegal>(r => r.Declaration.Id == request.DeclarationId,
                token);
            legal.Declaration.SetStatus(DeclarationStatus.Locked);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}