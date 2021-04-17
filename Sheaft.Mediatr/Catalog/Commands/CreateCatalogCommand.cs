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
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Catalog.Commands
{
    public class CreateCatalogCommand : Command<Guid>
    {
        public CreateCatalogCommand(RequestUser requestUser) : base(requestUser)
        {
            Kind = CatalogKind.Stores;
            ProducerId = requestUser.Id;
        }

        public string Name { get; set; }
        public CatalogKind Kind { get; set; }
        public bool IsDefault { get; set; }
        public bool IsAvailable { get; set; }
        public Guid ProducerId { get; set; }
    }

    public class CreateCatalogCommandHandler : CommandsHandler,
        IRequestHandler<CreateCatalogCommand, Result<Guid>>
    {
        public CreateCatalogCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CreateCatalogCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(CreateCatalogCommand request, CancellationToken token)
        {
            var producer = await _context.GetByIdAsync<Domain.Producer>(request.ProducerId, token);
            var catalogs =
                await _context.GetAsync<Domain.Catalog>(
                    c => c.Producer.Id == request.RequestUser.Id, token);
            
            if(request.Kind == CatalogKind.Consumers && catalogs.Any(c => c.Kind == CatalogKind.Consumers))
                throw SheaftException.AlreadyExists();
            
            var entity = new Domain.Catalog(producer, request.Kind, Guid.NewGuid(), request.Name);
            entity.SetIsAvailable(request.IsAvailable);
            entity.SetIsDefault(request.IsDefault);

            if (request.IsDefault && catalogs.Any(c => c.IsDefault && c.Kind == CatalogKind.Stores))
            {
                var actualDefaultCatalog = catalogs.Single(c => c.IsDefault && c.Kind == CatalogKind.Stores);
                actualDefaultCatalog.SetIsDefault(false);
            }

            await _context.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);
            return Success(entity.Id);
        }
    }
}