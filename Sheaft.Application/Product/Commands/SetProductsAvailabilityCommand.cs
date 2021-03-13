using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Product.Commands
{
    public class SetProductsAvailabilityCommand : Command
    {
        [JsonConstructor]
        public SetProductsAvailabilityCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> ProductIds { get; set; }
        public bool Available { get; set; }
    }

    public class SetProductsAvailabilityCommandHandler : CommandsHandler,
        IRequestHandler<SetProductsAvailabilityCommand, Result>
    {
        public SetProductsAvailabilityCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<SetProductsAvailabilityCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(SetProductsAvailabilityCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                foreach (var id in request.ProductIds)
                {
                    var result = await _mediatr.Process(
                        new SetProductAvailabilityCommand(request.RequestUser) {ProductId = id, Available = request.Available},
                        token);
                    if (!result.Succeeded)
                        return Failure(result.Exception);
                }

                await transaction.CommitAsync(token);
                return Success();
            }
        }
    }
}