using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Catalog
{
    public class SetCatalogAvailabilityCommand: Command
    {
        public SetCatalogAvailabilityCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        
        public Guid CatalogId { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class SetCatalogAvailabilityCommandHandler : CommandsHandler,
        IRequestHandler<SetCatalogAvailabilityCommand, Result>
    {
        public SetCatalogAvailabilityCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<SetCatalogAvailabilityCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(SetCatalogAvailabilityCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Catalog>(request.CatalogId, token);
            entity.SetIsAvailable(request.IsAvailable);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}