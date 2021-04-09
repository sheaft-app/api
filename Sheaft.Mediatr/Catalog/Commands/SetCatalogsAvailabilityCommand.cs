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

namespace Sheaft.Mediatr.Catalog
{
    public class SetCatalogsAvailabilityCommand: Command
    {
        public SetCatalogsAvailabilityCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        
        public bool IsAvailable { get; set; }
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
                foreach (var id in request.CatalogIds)
                {
                    var result = await _mediatr.Process(
                        new SetCatalogAvailabilityCommand(request.RequestUser)
                            {CatalogId = id, IsAvailable = request.IsAvailable}, token);

                    if (!result.Succeeded)
                        throw result.Exception;
                }

                await transaction.CommitAsync(token);
            }

            return Success();
        }
    }
}