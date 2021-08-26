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
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Catalog.Commands
{
    public class RemoveProductsFromCatalogCommand : Command
    {
        protected RemoveProductsFromCatalogCommand()
        {
            
        }
        public RemoveProductsFromCatalogCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid CatalogId { get; set; }
        public IEnumerable<Guid> ProductIds { get; set; }
    }

    public class RemoveProductsFromCatalogCommandHandler : CommandsHandler,
        IRequestHandler<RemoveProductsFromCatalogCommand, Result>
    {
        public RemoveProductsFromCatalogCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RemoveProductsFromCatalogCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RemoveProductsFromCatalogCommand request, CancellationToken token)
        {
            var products = await _context.Products
                .Where(p => request.ProductIds.Contains(p.Id))
                .ToListAsync(token);
            
            foreach (var product in products)
                product.RemoveFromCatalog(request.CatalogId);
            
            // var quickOrders = await _context.QuickOrders
            //     .Where(qo => qo.Products.Any(qop => !qop.CatalogProductId))
            //     .Include(qo => qo.Products)
            //     .ToListAsync(token);
            //
            // foreach (var quickOrder in quickOrders)
            // {
            //     quickOrder.RemoveUnboundProducts();
            // }
            
            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}