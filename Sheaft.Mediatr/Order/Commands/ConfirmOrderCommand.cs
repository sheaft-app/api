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
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.Donation.Commands;
using Sheaft.Mediatr.PreAuthorizedPayin;
using Sheaft.Mediatr.PreAuthorizedPayin.Commands;
using Sheaft.Mediatr.PurchaseOrder.Commands;
using Sheaft.Mediatr.User.Commands;

namespace Sheaft.Mediatr.Order.Commands
{
    public class ConfirmOrderCommand : Command<IEnumerable<Guid>>
    {
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
            var order = await _context.GetByIdAsync<Domain.Order>(request.OrderId, token);
            if(order.User == null)
                throw SheaftException.BadRequest(MessageKind.Order_CannotCreate_User_Required);

            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var purchaseOrderIds = new List<Guid>();
                order.SetStatus(OrderStatus.Validated);

                var producerIds = order.Products.Select(p => p.Producer.Id).Distinct();
                foreach (var producerId in producerIds)
                {
                    var result = await _mediatr.Process(new CreatePurchaseOrderCommand(request.RequestUser)
                    {
                        OrderId = order.Id,
                        ProducerId = producerId,
                        SkipNotification = true
                    }, token);

                    if (!result.Succeeded)
                        return Failure<IEnumerable<Guid>>(result);

                    purchaseOrderIds.Add(result.Data);
                }

                await transaction.CommitAsync(token);

                var preAuthorization = await _context.FindSingleAsync<Domain.PreAuthorization>(p =>
                    p.Order.Id == order.Id && p.Status == PreAuthorizationStatus.Succeeded &&
                    p.PaymentStatus == PaymentStatus.Validated, token);

                if (preAuthorization != null)
                {
                    TimeSpan diff = preAuthorization.ExpirationDate.Value.AddDays(-2) - DateTimeOffset.Now;
                    _mediatr.Schedule(new CreatePreAuthorizedPayinCommand(request.RequestUser)
                    {
                        PreAuthorizationId = preAuthorization.Id
                    }, TimeSpan.FromHours(diff.TotalHours));
                }

                _mediatr.Post(new CreateUserPointsCommand(request.RequestUser)
                {
                    CreatedOn = DateTimeOffset.UtcNow, Kind = PointKind.PurchaseOrder, UserId = order.User.Id
                });

                return Success(purchaseOrderIds.AsEnumerable());
            }
        }
    }
}