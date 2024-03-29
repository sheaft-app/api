﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Core.Options;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Agreement.Commands
{
    public class RestoreAgreementCommand : Command
    {
        protected RestoreAgreementCommand()
        {
            
        }
        [JsonConstructor]
        public RestoreAgreementCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid AgreementId { get; set; }
    }

    public class RestoreAgreementCommandsHandler : CommandsHandler,
        IRequestHandler<RestoreAgreementCommand, Result>
    {
        private readonly RoleOptions _roleOptions;

        public RestoreAgreementCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IOptionsSnapshot<RoleOptions> roleOptions,
            ILogger<RestoreAgreementCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
        }

        public async Task<Result> Handle(RestoreAgreementCommand request, CancellationToken token)
        {
            var entity =
                await _context.Agreements.SingleOrDefaultAsync(a => a.Id == request.AgreementId && a.RemovedOn.HasValue, token);
            
            if(request.RequestUser.IsInRole(_roleOptions.Producer.Value) && entity.ProducerId != request.RequestUser.Id)
                return Failure("Vous n'êtes pas autorisé à accéder à cette ressource.");
            
            if(request.RequestUser.IsInRole(_roleOptions.Store.Value) && entity.StoreId != request.RequestUser.Id)
                return Failure("Vous n'êtes pas autorisé à accéder à cette ressource.");

            if (await _context.Agreements.AnyAsync(
                a => a.Id != entity.Id && a.StoreId == entity.StoreId && a.ProducerId == entity.ProducerId &&
                     a.Status != AgreementStatus.Cancelled && a.Status != AgreementStatus.Refused, token))
                return Failure("Un partenariat est déjà actif ou en attente d'acceptation.");
            
            _context.Restore(entity);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}