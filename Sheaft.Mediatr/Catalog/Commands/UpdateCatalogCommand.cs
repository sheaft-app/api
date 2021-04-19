using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Catalog.Commands
{
    public class UpdateCatalogCommand: Command
    {
        protected UpdateCatalogCommand()
        {
            
        }
        public UpdateCatalogCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        
        public Guid CatalogId { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class UpdateCatalogCommandHandler : CommandsHandler,
        IRequestHandler<UpdateCatalogCommand, Result>
    {
        public UpdateCatalogCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateCatalogCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateCatalogCommand request, CancellationToken token)
        {
            var catalogs =
                await _context.Catalogs
                    .Where(c => c.Producer.Id == request.RequestUser.Id)
                    .ToListAsync(token);
            
            var entity = catalogs.Single(c => c.Id == request.CatalogId);
            if(!request.IsDefault && entity.Kind == CatalogKind.Consumers)
                return Failure(MessageKind.Validation);
                
            entity.SetIsAvailable(request.IsAvailable);
            
            if(entity.Kind != CatalogKind.Consumers)
                entity.SetIsDefault(request.IsDefault);

            if (entity.IsDefault && catalogs.Any(c => c.Id != entity.Id && c.IsDefault && c.Kind == CatalogKind.Stores))
            {
                var actualDefaultCatalog = catalogs.Single(c => c.Id != entity.Id && c.IsDefault && c.Kind == CatalogKind.Stores);
                actualDefaultCatalog.SetIsDefault(false);
            }

            if (catalogs.Where(c => c.Kind == CatalogKind.Stores).All(c => !c.IsDefault))
                return Failure(MessageKind.Validation);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}