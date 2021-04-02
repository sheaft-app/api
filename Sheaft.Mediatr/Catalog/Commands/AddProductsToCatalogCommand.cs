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

namespace Sheaft.Mediatr.Catalog
{
    public class AddProductsToCatalogCommand: Command
    {
        public AddProductsToCatalogCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        
        public Guid CatalogId { get; set; }
        public IEnumerable<UpdateOrCreateCatalogPriceDto> Products { get; set; }
    }

    public class AddProductsToCatalogCommandHandler : CommandsHandler,
        IRequestHandler<AddProductsToCatalogCommand, Result>
    {
        public AddProductsToCatalogCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<AddProductsToCatalogCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(AddProductsToCatalogCommand request, CancellationToken token)
        {
            var catalog = await _context.GetByIdAsync<Domain.Catalog>(request.CatalogId, token);

            var productIds = request.Products.Select(p => p.Id);
            var products = await _context.GetAsync<Domain.Product>(p => productIds.Contains(p.Id), token);

            foreach (var product in products)
            {
                var catalogProductPrice = request.Products.Single(p => p.Id == product.Id);
                catalog.AddProduct(product, catalogProductPrice.WholeSalePricePerUnit);
            }
            
            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}