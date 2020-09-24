using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Commands;
using Sheaft.Core;
using Sheaft.Domain.Models;
using Sheaft.Application.Interop;

namespace Sheaft.Application.Handlers
{
    public class QuickOrderCommandsHandler : ResultsHandler,
        IRequestHandler<CreateQuickOrderCommand, Result<Guid>>,
        IRequestHandler<UpdateQuickOrderCommand, Result<bool>>,
        IRequestHandler<UpdateQuickOrderProductsCommand, Result<bool>>,
        IRequestHandler<SetDefaultQuickOrderCommand, Result<bool>>,
        IRequestHandler<DeleteQuickOrderCommand, Result<bool>>,
        IRequestHandler<DeleteQuickOrdersCommand, Result<bool>>
    {
        public QuickOrderCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<QuickOrderCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(CreateQuickOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
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

        public async Task<Result<bool>> Handle(UpdateQuickOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<QuickOrder>(request.Id, token);

                entity.SetName(request.Name);
                entity.SetDescription(request.Name);

                _context.Update(entity);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(UpdateQuickOrderProductsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
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

                _context.Update(entity);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(SetDefaultQuickOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var quickOrders = await _context.FindAsync<QuickOrder>(c => c.User.Id == request.RequestUser.Id, token);
                foreach (var quickOrder in quickOrders)
                {
                    if (quickOrder.Id == request.Id)
                        quickOrder.SetAsDefault();
                    else
                        quickOrder.UnsetAsDefault();
                }

                _context.UpdateRange(quickOrders);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(DeleteQuickOrdersCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                foreach (var id in request.Ids)
                {
                    var result = await _mediatr.Process(new DeleteQuickOrderCommand(request.RequestUser) { Id = id }, token);
                    if (!result.Success)
                        return Failed<bool>(result.Exception);
                }

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(DeleteQuickOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<QuickOrder>(request.Id, token);
                _context.Remove(entity);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }
    }
}
