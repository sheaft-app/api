using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Domain;

namespace Sheaft.Application.QuickOrder.Commands
{
    public class UpdateQuickOrderProductsCommand : Command
    {
        [JsonConstructor]
        public UpdateQuickOrderProductsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid QuickOrderId { get; set; }
        public IEnumerable<ProductQuantityInput> Products { get; set; }
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

                foreach (var newProduct in products)
                {
                    var product = await _context.FindByIdAsync<Domain.Product>(newProduct.Id, token);
                    entity.AddProduct(new KeyValuePair<Domain.Product, int>(product, newProduct.Quantity));
                }
            }

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}