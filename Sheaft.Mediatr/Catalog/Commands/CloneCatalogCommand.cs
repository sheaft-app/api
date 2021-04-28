using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Catalog.Commands
{
    public class CloneCatalogCommand : Command<Guid>
    {
        protected CloneCatalogCommand()
        {
            
        }
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
                var catalog = await _context.Catalogs.SingleAsync(e => e.Id == request.CatalogId, token);

                var entity = new Domain.Catalog(catalog.Producer, catalog.Kind, Guid.NewGuid(), request.Name);
                await _context.AddAsync(entity, token);

                var catalogProducts =
                    await _context.Products
                        .Where(p => p.CatalogsPrices.Any(cp => cp.CatalogId == request.CatalogId))
                        .ToListAsync(token);

                var products = catalogProducts.Select(p =>
                    new ProductPriceInputDto
                    {
                        ProductId = p.Id,
                        WholeSalePricePerUnit =
                            p.CatalogsPrices.Single(c => c.CatalogId == request.CatalogId).WholeSalePricePerUnit *
                            (1 + request.Percent ?? 0)
                    });

                var result =
                    await _mediatr.Process(
                        new AddOrUpdateProductsToCatalogCommand(request.RequestUser)
                            {CatalogId = entity.Id, Products = products}, token);

                if (!result.Succeeded)
                    return Failure<Guid>(result);

                await transaction.CommitAsync(token);
                return Success(entity.Id);
            }
        }
    }
}