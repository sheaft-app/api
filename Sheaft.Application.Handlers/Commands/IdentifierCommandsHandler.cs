﻿using Sheaft.Application.Commands;
using Sheaft.Application.Interop;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Core;
using Microsoft.Extensions.Logging;

namespace Sheaft.Application.Handlers
{
    public class IdentifierCommandsHandler : ResultsHandler,
            IRequestHandler<CreatePurchaseOrderIdentifierCommand, Result<string>>,
            IRequestHandler<CreateProductIdentifierCommand, Result<string>>,
            IRequestHandler<CreateSponsoringCodeCommand, Result<string>>
    {
        private readonly IIdentifierService _identifierService;

        public IdentifierCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IIdentifierService identifierService,
            ILogger<IdentifierCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _identifierService = identifierService;
        }

        public async Task<Result<string>> Handle(CreatePurchaseOrderIdentifierCommand request, CancellationToken token)
        {
            return await ExecuteAsync(() => _identifierService.GetNextOrderReferenceAsync(request.ProducerId, token));
        }

        public async Task<Result<string>> Handle(CreateProductIdentifierCommand request, CancellationToken token)
        {
            return await ExecuteAsync(() => _identifierService.GetNextProductReferenceAsync(request.ProducerId, token));
        }

        public async Task<Result<string>> Handle(CreateSponsoringCodeCommand request, CancellationToken token)
        {
            return await ExecuteAsync(() => _identifierService.GetNextSponsoringCode(token));
        }
    }
}