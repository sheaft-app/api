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
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Options;

namespace Sheaft.Mediatr.Agreement.Commands
{
    public class RefuseAgreementCommand : Command
    {
        protected RefuseAgreementCommand()
        {
            
        }
        [JsonConstructor]
        public RefuseAgreementCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid AgreementId { get; set; }
        public string Reason { get; set; }
    }

    public class RefuseAgreementCommandsHandler : CommandsHandler,
        IRequestHandler<RefuseAgreementCommand, Result>
    {
        private readonly RoleOptions _roleOptions;

        public RefuseAgreementCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IOptionsSnapshot<RoleOptions> roleOptions,
            ILogger<RefuseAgreementCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
        }

        public async Task<Result> Handle(RefuseAgreementCommand request, CancellationToken token)
        {
            var entity = await _context.Agreements.SingleAsync(e => e.Id == request.AgreementId, token);
            if(request.RequestUser.IsInRole(_roleOptions.Producer.Value) && entity.ProducerId != request.RequestUser.Id)
                return Failure("Vous n'êtes pas autorisé à accéder à cette ressource.");
            
            if(request.RequestUser.IsInRole(_roleOptions.Store.Value) && entity.StoreId != request.RequestUser.Id)
                return Failure("Vous n'êtes pas autorisé à accéder à cette ressource.");
            
            var currentUser = await _context.Users.SingleAsync(c => c.Id == request.RequestUser.Id, token);
            entity.RefuseAgreement(request.Reason, currentUser.Kind);
            
            await _context.SaveChangesAsync(token);
            return Success(true);
        }
    }
}