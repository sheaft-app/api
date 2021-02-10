using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Models;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class CreateQuickOrderCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateQuickOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<ProductQuantityInput> Products { get; set; }
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
            return await ExecuteAsync(request, async () =>
            {
                var user = await _context.GetByIdAsync<User>(request.RequestUser.Id, token);

                var productIds = request.Products.Select(p => p.Id);
                var products = await _context.GetByIdsAsync<Product>(productIds, token);

                var cartProducts = products
                                            .Select(c => new { Product = c, Quantity = request.Products.Where(p => p.Id == c.Id).Sum(c => c.Quantity) })
                                            .ToDictionary(d => d.Product, d => d.Quantity);

                var quickOrder = new QuickOrder(Guid.NewGuid(), request.Name, cartProducts, user);

                await _context.AddAsync(quickOrder, token);
                await _context.SaveChangesAsync(token);

                return Created(quickOrder.Id);
            });
        }
    }
}
