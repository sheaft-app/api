using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Catalog.Commands
{
    public class SetCatalogsAvailabilityCommand: Command
    {
        protected SetCatalogsAvailabilityCommand()
        {
            
        }
        public SetCatalogsAvailabilityCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        
        public bool Available { get; set; }
        public IEnumerable<Guid> CatalogIds { get; set; }
    }

    public class SetCatalogsAvailabilityCommandHandler : CommandsHandler,
        IRequestHandler<SetCatalogsAvailabilityCommand, Result>
    {
        public SetCatalogsAvailabilityCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<SetCatalogsAvailabilityCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(SetCatalogsAvailabilityCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                Result result = null;
                foreach (var id in request.CatalogIds)
                {
                    result = await _mediatr.Process(
                        new SetCatalogAvailabilityCommand(request.RequestUser)
                            {CatalogId = id, IsAvailable = request.Available}, token);

                    if (!result.Succeeded)
                        break;
                }

                if (result is {Succeeded: false})
                    return result;

                await transaction.CommitAsync(token);
            }

            return Success();
        }
    }
}