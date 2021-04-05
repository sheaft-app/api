using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Catalog
{
    public class CloneCatalogCommand : Command<Guid>
    {
        public CloneCatalogCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid CatalogId { get; set; }
        public string Name { get; set; }
        public decimal? Percent { get; set; }
    }

    public class CloneCatalogCommandHandler : CommandsHandler,
        IRequestHandler<CloneCatalogCommand, Result<Guid>>
    {
        public CloneCatalogCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CloneCatalogCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(CloneCatalogCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var catalog = await _context.GetByIdAsync<Domain.Catalog>(request.CatalogId, token);

                var entity = new Domain.Catalog(catalog.Producer, catalog.Kind, Guid.NewGuid(), request.Name);
                await _context.AddAsync(entity, token);

                var catalogProducts =
                    await _context.GetAsync<Domain.Product>(
                        p => p.CatalogsPrices.Any(cp => cp.Catalog.Id == request.CatalogId), token);
                
                var products = catalogProducts.Select(p =>
                    new UpdateOrCreateCatalogPriceDto
                    {
                        Id = p.Id, WholeSalePricePerUnit = p.CatalogsPrices.Single(c => c.Catalog.Id == request.CatalogId).WholeSalePricePerUnit * (1 + request.Percent ?? 0)
                    });

                var result =
                    await _mediatr.Process(
                        new AddProductsToCatalogCommand(request.RequestUser)
                            {CatalogId = entity.Id, Products = products}, token);

                if (!result.Succeeded)
                    return Failure<Guid>(result.Exception);

                await transaction.CommitAsync(token);
                return Success(entity.Id);
            }
        }
    }
}