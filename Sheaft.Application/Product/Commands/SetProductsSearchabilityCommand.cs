using System;
using System.Collections.Generic;
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

namespace Sheaft.Application.Product.Commands
{
    public class SetProductsSearchabilityCommand : Command
    {
        [JsonConstructor]
        public SetProductsSearchabilityCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
        public bool VisibleToStores { get; set; }
        public bool VisibleToConsumers { get; set; }
    }

    public class SetProductsSearchabilityCommandHandler : CommandsHandler,
        IRequestHandler<SetProductsSearchabilityCommand, Result>
    {
        private readonly IBlobService _blobService;

        public SetProductsSearchabilityCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            ILogger<SetProductsSearchabilityCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
        }

        public async Task<Result> Handle(SetProductsSearchabilityCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                foreach (var id in request.Ids)
                {
                    var result = await _mediatr.Process(
                        new SetProductSearchabilityCommand(request.RequestUser)
                        {
                            Id = id, VisibleToStores = request.VisibleToStores,
                            VisibleToConsumers = request.VisibleToConsumers
                        }, token);
                    if (!result.Succeeded)
                        return Failure(result.Exception);
                }

                await transaction.CommitAsync(token);
                return Success();
            }
        }
    }
}