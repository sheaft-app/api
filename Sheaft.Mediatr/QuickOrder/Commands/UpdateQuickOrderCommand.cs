using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.QuickOrder.Commands
{
    public class UpdateQuickOrderCommand : Command
    {
        protected UpdateQuickOrderCommand()
        {
            
        }
        [JsonConstructor]
        public UpdateQuickOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid QuickOrderId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }
        public IEnumerable<ResourceIdQuantityInputDto> Products { get; set; }
    }

    public class UpdateQuickOrderCommandHandler : CommandsHandler,
        IRequestHandler<UpdateQuickOrderCommand, Result>
    {
        public UpdateQuickOrderCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateQuickOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateQuickOrderCommand request, CancellationToken token)
        {
            var entity = await _context.QuickOrders
                .Include(qo => qo.Products)
                    .ThenInclude(qp => qp.CatalogProduct)
                        .ThenInclude(qcp => qcp.Product)
                .SingleAsync(e => e.Id == request.QuickOrderId, token);
            
            entity.SetName(request.Name);
            entity.SetDescription(request.Description);
            entity.SetIsDefault(request.IsDefault);
            
            if(request.Products == null)
                request.Products = new List<ResourceIdQuantityInputDto>();

            var productIds = request.Products.Select(p => p.Id).ToList();   
            var existingProductIds = entity.Products.Select(p => p.CatalogProduct.ProductId);
            
            var productToRemoveIds = existingProductIds.Except(productIds);
            foreach (var productToRemoveId in productToRemoveIds)
            {
                var quickOrderProduct = entity.RemoveProduct(productToRemoveId);
                _context.Remove(quickOrderProduct);
            }
            
            var agreements = await _context.Set<Domain.Agreement>()
                .Where(a => a.StoreId == entity.UserId && a.Catalog.Products.Any(p => productIds.Contains(p.ProductId)))
                .Include(a => a.Catalog)
                    .ThenInclude(c => c.Products)
                        .ThenInclude(c => c.Product)
                .ToListAsync(token);

            Result result = null;
            foreach (var productId in productIds)
            {
                var quantity = request.Products.FirstOrDefault(p => p.Id == productId)?.Quantity ?? null;
                var catalogProduct =
                    agreements.Where(a => a.Catalog.Products.Any(p => p.ProductId == productId))
                        .SelectMany(c => c.Catalog.Products)
                        .FirstOrDefault(cp => cp.ProductId == productId);

                if (catalogProduct != null)
                {
                    entity.AddOrUpdateProduct(catalogProduct, quantity);
                    continue;
                }

                result = Failure("Le produit n'est pas disponible dans vos partenariats.");
                break;
            }

            if (result is {Succeeded: false})
                return result;

            var oldDefaultQuickOrder =
                await _context.QuickOrders.SingleOrDefaultAsync(
                    qo => qo.UserId == entity.UserId && qo.Id != entity.Id && qo.IsDefault, token);
            if(oldDefaultQuickOrder != null)
                oldDefaultQuickOrder.SetIsDefault(false);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}