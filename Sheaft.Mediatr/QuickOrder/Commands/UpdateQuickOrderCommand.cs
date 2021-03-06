﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
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
            var entity = await _context.QuickOrders.SingleAsync(e => e.Id == request.QuickOrderId, token);
            entity.SetName(request.Name);
            entity.SetDescription(request.Description);
            
            if (request.Products != null && request.Products.Any())
            {
                var productIds = request.Products.Select(p => p.Id).ToList();
                var agreements = await _context.Set<Domain.Agreement>()
                    .Where(a => a.StoreId == entity.UserId && a.Catalog.Products.Any(p => productIds.Contains(p.ProductId)))
                    .Include(a => a.Catalog)
                    .ThenInclude(c => c.Products)
                    .ThenInclude(c => c.Product)
                    .ToListAsync(token);

                var existingProductIds = entity.Products.Select(p => p.CatalogProduct.ProductId);
                
                var productToRemoveIds = productIds.Except(existingProductIds);
                foreach (var productToRemoveId in productToRemoveIds)
                    entity.RemoveProduct(productToRemoveId);

                Result result = null;
                foreach (var productId in productIds)
                {
                    var quantity = request.Products.Where(p => p.Id == productId).Sum(p => p.Quantity);
                    var catalogProduct =
                        agreements.Where(a => a.Catalog.Products.Any(p => p.ProductId == productId))
                            .Select(a => a.Catalog)
                            .SelectMany(c => c.Products)
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
            }

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}