using System;
using System.Collections.Generic;
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

namespace Sheaft.Mediatr.Product.Commands
{
    public class SetProductsSearchabilityCommand : Command
    {
        [JsonConstructor]
        public SetProductsSearchabilityCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> ProductIds { get; set; }
        public bool VisibleToStores { get; set; }
        public bool VisibleToConsumers { get; set; }
    }

    public class SetProductsSearchabilityCommandHandler : CommandsHandler,
        IRequestHandler<SetProductsSearchabilityCommand, Result>
    {
        public SetProductsSearchabilityCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<SetProductsSearchabilityCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(SetProductsSearchabilityCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                foreach (var id in request.ProductIds)
                {
                    var result = await _mediatr.Process(
                        new SetProductSearchabilityCommand(request.RequestUser)
                        {
                            ProductId = id, VisibleToStores = request.VisibleToStores,
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