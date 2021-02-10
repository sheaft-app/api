using Sheaft.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class UpdateQuickOrderProductsCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateQuickOrderProductsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public IEnumerable<ProductQuantityInput> Products { get; set; }
    }
    
    public class UpdateQuickOrderProductsCommandHandler : CommandsHandler,
        IRequestHandler<UpdateQuickOrderProductsCommand, Result<bool>>
    {
        public UpdateQuickOrderProductsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateQuickOrderProductsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(UpdateQuickOrderProductsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<QuickOrder>(request.Id, token);
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

                    foreach (var newProduct in products)
                    {
                        var product = await _context.FindByIdAsync<Product>(newProduct.Id, token);
                        entity.AddProduct(new KeyValuePair<Product, int>(product, newProduct.Quantity));
                    }
                }

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}
