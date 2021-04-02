using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.QuickOrder.Commands
{
    public class UpdateQuickOrderProductsCommand : Command
    {
        [JsonConstructor]
        public UpdateQuickOrderProductsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid QuickOrderId { get; set; }
        public IEnumerable<ResourceIdQuantityDto> Products { get; set; }
    }

    public class UpdateQuickOrderProductsCommandHandler : CommandsHandler,
        IRequestHandler<UpdateQuickOrderProductsCommand, Result>
    {
        public UpdateQuickOrderProductsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateQuickOrderProductsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateQuickOrderProductsCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.QuickOrder>(request.QuickOrderId, token);
            if (request.Products != null && request.Products.Any())
            {
                var products = request.Products.ToList();
                foreach (var orderProduct in entity.Products.ToList())
                {
                    var productToUpdate = request.Products.SingleOrDefault(p => p.Id == orderProduct.Product.Id);
                    if (productToUpdate == null)
                        entity.RemoveProduct(orderProduct.Product);
                    else
                    {
                        orderProduct.SetQuantity(productToUpdate.Quantity);
                    }

                    products.Remove(productToUpdate);
                }

                var productIds = products.Select(p => p.Id).ToList();
                var existingProducts = await _context.GetAsync<Domain.Product>(
                    p => productIds.Contains(p.Id) && p.CatalogsPrices.Any(cp => cp.Catalog.Kind == CatalogKind.Stores),
                    token);
                
                foreach (var newProduct in products)
                {
                    var product = existingProducts.Single(ep => ep.Id == newProduct.Id);
                    entity.AddProduct(new KeyValuePair<Domain.Product, int>(product, newProduct.Quantity));
                }
            }

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}