using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Catalog.Commands
{
    public class RemoveProductsFromCatalogCommand: Command
    {
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
            var products = await _context.GetAsync<Domain.Product>(p => request.ProductIds.Contains(p.Id), token);
            foreach (var product in products)
                product.RemoveFromCatalog(request.CatalogId);
            
            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}