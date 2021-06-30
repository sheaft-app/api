using System;
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
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.QuickOrder.Commands
{
    public class CreateQuickOrderCommand : Command<Guid>
    {
        protected CreateQuickOrderCommand()
        {
            
        }
        [JsonConstructor]
        public CreateQuickOrderCommand(RequestUser requestUser) : base(requestUser)
        {
            UserId = RequestUser.Id;
        }

        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<ResourceIdQuantityInputDto> Products { get; set; }

        public override void SetRequestUser(RequestUser user)
        {
            base.SetRequestUser(user);
            UserId = RequestUser.Id;
        }
    }

    public class CreateQuickOrderCommandHandler : CommandsHandler,
        IRequestHandler<CreateQuickOrderCommand, Result<Guid>>
    {
        public CreateQuickOrderCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CreateQuickOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(CreateQuickOrderCommand request, CancellationToken token)
        {
            var store = await _context.Users.SingleAsync(e => e.Id == request.UserId, token);
            var productIds = request.Products.Select(p => p.Id).ToList();
            var agreements = await _context.Set<Domain.Agreement>()
                .Where(a => a.StoreId == store.Id && a.Catalog.Products.Any(p => productIds.Contains(p.ProductId)))
                .Include(a => a.Catalog)
                    .ThenInclude(c => c.Products)
                        .ThenInclude(c => c.Product)
                .ToListAsync(token);

            var catalogProducts = new Dictionary<CatalogProduct, int>();
            Result<Guid> result = null;
            foreach (var productId in productIds)
            {
                var quantity = request.Products.Where(p => p.Id == productId).Sum(p => p.Quantity);
                var catalogProduct =
                    agreements.Where(a => a.Catalog.Products.Any(p => p.Product.Id == productId))
                        .Select(a => a.Catalog)
                        .SelectMany(c => c.Products)
                        .FirstOrDefault(cp => cp.Product.Id == productId);

                if (catalogProduct != null)
                {
                    catalogProducts.Add(catalogProduct, quantity);
                    continue;
                }

                result = Failure<Guid>("Le produit n'est pas disponible dans vos partenariats.");
                break;
            }

            if (result is {Succeeded: false})
                return result;
            
            var quickOrder = new Domain.QuickOrder(Guid.NewGuid(), request.Name, catalogProducts, store);

            await _context.AddAsync(quickOrder, token);
            await _context.SaveChangesAsync(token);

            return Success(quickOrder.Id);
        }
    }
}