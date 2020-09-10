using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Commands;
using Sheaft.Core;
using Sheaft.Infrastructure.Interop;
using Microsoft.Extensions.Logging;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Handlers
{
    public class OrderCommandsHandler : CommandsHandler,
        IRequestHandler<CreateOrderCommand, Result<Guid>>
    {
        private readonly IAppDbContext _context;

        public OrderCommandsHandler(
            IAppDbContext context,
            ILogger<PurchaseOrderCommandsHandler> logger) : base(logger)
        {
            _context = context;
        }

        public async Task<Result<Guid>> Handle(CreateOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var user = await _context.GetByIdAsync<User>(request.RequestUser.Id, token);
                var order = new Order(Guid.NewGuid(), user, request.Donation);

                await _context.AddAsync(order);
                await _context.SaveChangesAsync(token);

                return Ok(order.Id);
            });
        }
    }
}
