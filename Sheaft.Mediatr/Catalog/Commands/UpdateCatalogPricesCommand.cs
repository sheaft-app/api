using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Catalog.Commands
{
    public class UpdateCatalogPricesCommand: Command
    {
        protected UpdateCatalogPricesCommand()
        {
            
        }
        public UpdateCatalogPricesCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        
        public Guid CatalogId { get; set; }
        public IEnumerable<ProductPriceInputDto> Prices { get; set; }
    }

    public class UpdateCatalogPricesCommandHandler : CommandsHandler,
        IRequestHandler<UpdateCatalogPricesCommand, Result>
    {
        public UpdateCatalogPricesCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateCatalogPricesCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateCatalogPricesCommand request, CancellationToken token)
        {
            var productIds = request.Prices.Select(p => p.ProductId);
            var catalogProductsPrice = await _context.Set<CatalogProduct>()
                .Where(c => c.CatalogId == request.CatalogId && !c.Catalog.RemovedOn.HasValue && productIds.Contains(c.Product.Id))
                .Include(c => c.Product)
                .ToListAsync(token);

            foreach (var catalogProductPrice in catalogProductsPrice)
            {
                var newProductPrice = request.Prices.Single(p => p.ProductId == catalogProductPrice.Product.Id);
                catalogProductPrice.SetWholeSalePricePerUnit(newProductPrice.WholeSalePricePerUnit);
            }

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}