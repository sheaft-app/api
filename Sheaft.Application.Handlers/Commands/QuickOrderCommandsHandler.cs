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
using Sheaft.Infrastructure.Interop;

namespace Sheaft.Application.Handlers
{
    public class QuickOrderCommandsHandler : CommandsHandler,
        IRequestHandler<CreateQuickOrderCommand, CommandResult<Guid>>,
        IRequestHandler<UpdateQuickOrderCommand, CommandResult<bool>>,
        IRequestHandler<UpdateQuickOrderProductsCommand, CommandResult<bool>>,
        IRequestHandler<UpdateQuickOrderDeliveriesCommand, CommandResult<bool>>,
        IRequestHandler<SetDefaultQuickOrderCommand, CommandResult<bool>>,
        IRequestHandler<DeleteQuickOrderCommand, CommandResult<bool>>,
        IRequestHandler<DeleteQuickOrdersCommand, CommandResult<bool>>
    {
        private readonly IAppDbContext _context;
        private readonly IMediator _mediatr;

        public QuickOrderCommandsHandler(
            IMediator mediatr, 
            IAppDbContext context,
            ILogger<QuickOrderCommandsHandler> logger) : base(logger)
        {
            _mediatr = mediatr;
            _context = context;
        }

        public async Task<CommandResult<Guid>> Handle(CreateQuickOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var fromUser = await _context.GetByIdAsync<User>(request.RequestUser.Id, token);

                var productIds = request.Products.Select(p => p.Id);
                var products = await _context.GetByIdsAsync<Product>(productIds, token);

                var cartProducts = products
                                            .Select(c => new { Product = c, Quantity = request.Products.Where(p => p.Id == c.Id).Sum(c => c.Quantity) })
                                            .ToDictionary(d => d.Product, d => d.Quantity);

                var quickOrder = new QuickOrder(Guid.NewGuid(), request.Name, cartProducts, fromUser);

                await _context.AddAsync(quickOrder, token);
                await _context.SaveChangesAsync(token);

                return CreatedResult(quickOrder.Id);
            });
        }

        public async Task<CommandResult<bool>> Handle(UpdateQuickOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<QuickOrder>(request.Id, token);

                entity.SetName(request.Name);
                entity.SetDescription(request.Name);

                _context.Update(entity);

                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(UpdateQuickOrderProductsCommand request, CancellationToken token)
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

                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(UpdateQuickOrderDeliveriesCommand request, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task<CommandResult<bool>> Handle(SetDefaultQuickOrderCommand request, CancellationToken token)
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

                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(DeleteQuickOrdersCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                foreach (var id in request.Ids)
                {
                    var result = await _mediatr.Send(new DeleteQuickOrderCommand(request.RequestUser) { Id = id });
                    if (!result.Success)
                        return CommandFailed<bool>(result.Exception);
                }

                return OkResult(true);
            });
        }

        public async Task<CommandResult<bool>> Handle(DeleteQuickOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<QuickOrder>(request.Id, token);
                _context.Remove(entity);

                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }
    }
}
