using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Catalog.Commands
{
    public class CreateCatalogCommand : Command<Guid>
    {
        protected CreateCatalogCommand()
        {
            
        }
        public CreateCatalogCommand(RequestUser requestUser) : base(requestUser)
        {
            ProducerId = RequestUser.Id;
        }

        public string Name { get; set; }
        public CatalogKind Kind { get; set; }
        public bool IsDefault { get; set; }
        public bool IsAvailable { get; set; }
        public Guid ProducerId { get; set; }
        public IEnumerable<ProductPriceInputDto> Products { get; set; }

        public override void SetRequestUser(RequestUser user)
        {
            base.SetRequestUser(user);
            ProducerId = RequestUser.Id;
        }
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
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var catalogs =
                    await _context.Catalogs
                        .Where(c => c.Producer.Id == request.RequestUser.Id)
                        .ToListAsync(token);

                if (request.Kind == CatalogKind.Consumers && catalogs.Any(c => c.Kind == CatalogKind.Consumers))
                    return Failure<Guid>("Le catalogue pour les particuliers existe déjà.");

                var producer = await _context.Producers.SingleAsync(e => e.Id == request.ProducerId, token);
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
                
                var result =
                    await _mediatr.Process(
                        new AddOrUpdateProductsToCatalogCommand(request.RequestUser)
                            {CatalogId = entity.Id, Products = request.Products}, token);

                if (!result.Succeeded)
                    return Failure<Guid>(result);

                await transaction.CommitAsync(token);
                return Success(entity.Id);
            }
        }
    }
}