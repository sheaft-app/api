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
using Sheaft.Domain;

namespace Sheaft.Mediatr.Catalog.Commands
{
    public class UpdateAllCatalogPricesCommand: Command
    {
        protected UpdateAllCatalogPricesCommand()
        {
            
        }
        public UpdateAllCatalogPricesCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        
        public Guid CatalogId { get; set; }
        public decimal Percent { get; set; }
    }

    public class UpdateAllCatalogPricesCommandHandler : CommandsHandler,
        IRequestHandler<UpdateAllCatalogPricesCommand, Result>
    {
        public UpdateAllCatalogPricesCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateAllCatalogPricesCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateAllCatalogPricesCommand request, CancellationToken token)
        {
            var catalogProductsPrice = await _context.Set<CatalogProduct>()
                .Where(c => c.CatalogId == request.CatalogId && !c.Catalog.RemovedOn.HasValue)
                .Include(c => c.Product)
                .ToListAsync(token);

            foreach (var catalogProductPrice in catalogProductsPrice)
                catalogProductPrice.UpdatePrice(request.Percent);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}