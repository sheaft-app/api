using System;
using System.Collections.Generic;
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

namespace Sheaft.Mediatr.Catalog.Commands
{
    public class AddOrUpdateProductsToCatalogCommand: Command
    {
        public AddOrUpdateProductsToCatalogCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        
        public Guid CatalogId { get; set; }
        public IEnumerable<UpdateOrCreateCatalogPriceDto> Products { get; set; }
    }

    public class AddOrUpdateProductsToCatalogCommandHandler : CommandsHandler,
        IRequestHandler<AddOrUpdateProductsToCatalogCommand, Result>
    {
        public AddOrUpdateProductsToCatalogCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<AddOrUpdateProductsToCatalogCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(AddOrUpdateProductsToCatalogCommand request, CancellationToken token)
        {
            var catalog = await _context.GetByIdAsync<Domain.Catalog>(request.CatalogId, token);

            var productIds = request.Products.Select(p => p.Id);
            var products = await _context.GetAsync<Domain.Product>(p => productIds.Contains(p.Id), token);

            foreach (var product in products)
            {
                var catalogProductPrice = request.Products.Single(p => p.Id == product.Id);
                product.AddOrUpdateCatalogPrice(catalog, catalogProductPrice.WholeSalePricePerUnit);
            }
            
            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}