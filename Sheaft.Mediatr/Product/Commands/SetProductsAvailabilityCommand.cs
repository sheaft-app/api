using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Product.Commands
{
    public class SetProductsAvailabilityCommand : Command
    {
        protected SetProductsAvailabilityCommand()
        {
            
        }
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
                Result result = null;
                foreach (var id in request.ProductIds)
                {
                    result = await _mediatr.Process(
                        new SetProductAvailabilityCommand(request.RequestUser) {ProductId = id, Available = request.Available}, token);
                    
                    if (!result.Succeeded)
                        break;
                }

                if (result is {Succeeded: false})
                    return result;
                
                await transaction.CommitAsync(token);
                return Success();
            }
        }
    }
}