﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Options;

namespace Sheaft.Mediatr.Agreement.Commands
{
    public class AssignCatalogToAgreementCommand : Command
    {
        [JsonConstructor]
        public AssignCatalogToAgreementCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid AgreementId { get; set; }
        public Guid CatalogId { get; set; }
    }

    public class AssignCatalogToAgreementCommandHandler : CommandsHandler,
        IRequestHandler<AssignCatalogToAgreementCommand, Result>
    {
        private readonly RoleOptions _roleOptions;
        
        public AssignCatalogToAgreementCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IOptionsSnapshot<RoleOptions> roleOptions,
            ILogger<AssignCatalogToAgreementCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
        }

        public async Task<Result> Handle(AssignCatalogToAgreementCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Agreement>(request.AgreementId, token);
            if(request.RequestUser.IsInRole(_roleOptions.Producer.Value) && entity.Delivery.Producer.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();
            
            if(request.RequestUser.IsInRole(_roleOptions.Store.Value) && entity.Store.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            var catalog = await _context.GetSingleAsync<Domain.Catalog>(c => c.IsDefault && c.Kind == CatalogKind.Stores && c.Producer.Id == entity.Delivery.Producer.Id, token);
            entity.AssignCatalog(catalog);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}