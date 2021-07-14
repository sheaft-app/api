using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.Payin.Commands;
using Sheaft.Mediatr.PurchaseOrder.Commands;
using Sheaft.Mediatr.User.Commands;

namespace Sheaft.Mediatr.Order.Commands
{
    public class ConfirmOrderCommand : Command<IEnumerable<Guid>>
    {
        protected ConfirmOrderCommand()
        {
            
        }
        [JsonConstructor]
        public ConfirmOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid OrderId { get; set; }
    }

    public class ConfirmOrderCommandHandler : CommandsHandler,
        IRequestHandler<ConfirmOrderCommand, Result<IEnumerable<Guid>>>
    {
        public ConfirmOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<ConfirmOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<IEnumerable<Guid>>> Handle(ConfirmOrderCommand request, CancellationToken token)
        {
            var order = await _context.Orders.SingleAsync(e => e.Id == request.OrderId, token);
            if(order.User == null)
                throw SheaftException.BadRequest("Impossible de confirmer le panier, l'utilisateur est requis.");
            
            if (order.Processed)
                return Success(order.PurchaseOrders.Select(po => po.Id));

            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var purchaseOrderIds = new List<Result<Guid>>();
                order.SetStatus(OrderStatus.Confirmed);
                order.SetAsProcessed();

                var producerIds = order.Products.Select(p => p.Producer.Id).Distinct();
                foreach (var producerId in producerIds)
                {
                    var result = await _mediatr.Process(new CreatePurchaseOrderFromOrderCommand(request.RequestUser)
                    {
                        OrderId = order.Id,
                        ProducerId = producerId,
                        SkipNotification = true
                    }, token);

                    purchaseOrderIds.Add(result);
                }
                
                if(purchaseOrderIds.Any(p => !p.Succeeded))
                    return Failure<IEnumerable<Guid>>(purchaseOrderIds.First(p => !p.Succeeded));

                await _context.SaveChangesAsync(token);
                await transaction.CommitAsync(token);

                var preAuthorization = await _context.PreAuthorizations.SingleOrDefaultAsync(p =>
                    p.OrderId == order.Id && p.Status == PreAuthorizationStatus.Succeeded, token);

                if (preAuthorization == null) 
                    return Success(purchaseOrderIds.Select(p => p.Data));
                
                var expirationDate = preAuthorization.ExpirationDate ?? preAuthorization.CreatedOn.AddDays(7);
                var diff = expirationDate.AddDays(-2) - DateTimeOffset.Now;
                    
                _mediatr.Schedule(new CreatePreAuthorizedPayinCommand(request.RequestUser)
                {
                    PreAuthorizationId = preAuthorization.Id
                }, TimeSpan.FromMinutes(diff.TotalMinutes));

                return Success(purchaseOrderIds.Select(p => p.Data));
            }
        }
    }
}