﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Options;

namespace Sheaft.Mediatr.Agreement.Commands
{
    public class ResetAgreementStatusToCommand : Command
    {
        [JsonConstructor]
        public ResetAgreementStatusToCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid AgreementId { get; set; }
    }

    public class ResetAgreementCommandsHandler : CommandsHandler,
        IRequestHandler<ResetAgreementStatusToCommand, Result>
    {
        private readonly RoleOptions _roleOptions;

        public ResetAgreementCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IOptionsSnapshot<RoleOptions> roleOptions,
            ILogger<ResetAgreementCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
        }

        public async Task<Result> Handle(ResetAgreementStatusToCommand request, CancellationToken token)
        {
            var entity = await _context.Agreements.SingleOrDefaultAsync(a => a.Id == request.AgreementId, token);
            if(request.RequestUser.IsInRole(_roleOptions.Producer.Value) && entity.Delivery.Producer.Id != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);
            
            if(request.RequestUser.IsInRole(_roleOptions.Store.Value) && entity.Store.Id != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);

            entity.Reset();
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}