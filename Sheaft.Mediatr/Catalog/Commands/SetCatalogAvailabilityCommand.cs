using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Catalog.Commands
{
    public class SetCatalogAvailabilityCommand: Command
    {
        protected SetCatalogAvailabilityCommand()
        {
            
        }
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
            var entity = await _context.Catalogs.SingleAsync(e => e.Id == request.CatalogId, token);
            entity.SetIsAvailable(request.IsAvailable);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}