using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Catalog.Commands
{
    public class SetCatalogAsDefaultCommand: Command
    {
        protected SetCatalogAsDefaultCommand()
        {
            
        }
        public SetCatalogAsDefaultCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        
        public Guid CatalogId { get; set; }
    }

    public class SetCatalogAsDefaultCommandHandler : CommandsHandler,
        IRequestHandler<SetCatalogAsDefaultCommand, Result>
    {
        public SetCatalogAsDefaultCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<SetCatalogAsDefaultCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(SetCatalogAsDefaultCommand request, CancellationToken token)
        {
            var catalogs =
                await _context.Catalogs
                    .Where(c => c.ProducerId == request.RequestUser.Id)
                    .ToListAsync(token);
            
            var entity = catalogs.Single(c => c.Id == request.CatalogId);
            if(entity.Kind != CatalogKind.Stores)
                return Failure("Vous ne pouvez pas assigner un catalogue pour particulier comme catalogue par défaut.");
            
            entity.SetIsDefault(true);

            if (catalogs.Any(c => c.Id != request.CatalogId && c.IsDefault && c.Kind == CatalogKind.Stores))
            {
                var actualDefaultCatalog = catalogs.Single(c => c.Id != request.CatalogId && c.IsDefault && c.Kind == CatalogKind.Stores);
                actualDefaultCatalog.SetIsDefault(false);
            }

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}