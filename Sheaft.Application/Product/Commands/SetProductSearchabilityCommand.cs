using System;
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
using Sheaft.Domain.Exceptions;

namespace Sheaft.Application.Product.Commands
{
    public class SetProductSearchabilityCommand : Command
    {
        [JsonConstructor]
        public SetProductSearchabilityCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProductId { get; set; }
        public bool VisibleToStores { get; set; }
        public bool VisibleToConsumers { get; set; }
    }

    public class SetProductSearchabilityCommandHandler : CommandsHandler,
        IRequestHandler<SetProductSearchabilityCommand, Result>
    {
        public SetProductSearchabilityCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<SetProductSearchabilityCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(SetProductSearchabilityCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Product>(request.ProductId, token);
            if(entity.Producer.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            entity.SetConsumerVisibility(request.VisibleToConsumers);
            entity.SetStoreVisibility(request.VisibleToStores);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}