using System;
using Sheaft.Core;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Commands
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
        private readonly ICapingDeliveriesService _capingDeliveriesService;
        private readonly PspOptions _pspOptions;
        private readonly RoutineOptions _routineOptions;

        public ConfirmOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ICapingDeliveriesService capingDeliveriesService,
            IOptionsSnapshot<PspOptions> pspOptions,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<ConfirmOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _capingDeliveriesService = capingDeliveriesService;
            _pspOptions = pspOptions.Value;
            _routineOptions = routineOptions.Value;
        }

        public async Task<Result<IEnumerable<Guid>>> Handle(ConfirmOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                try
                {
                    using (var transaction = await _context.BeginTransactionAsync(token))
                    {
                        var order = await _context.GetByIdAsync<Order>(request.OrderId, token);
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

                            if (!result.Success)
                                return Failed<IEnumerable<Guid>>(result.Exception);

                            purchaseOrderIds.Add(result.Data);
                        }

                        await _context.SaveChangesAsync(token);
                        await transaction.CommitAsync(token);

                        foreach (var purchaseOrderId in purchaseOrderIds)
                        {
                            _mediatr.Post(new PurchaseOrderCreatedEvent(request.RequestUser) { PurchaseOrderId = purchaseOrderId });
                            _mediatr.Post(new PurchaseOrderReceivedEvent(request.RequestUser) { PurchaseOrderId = purchaseOrderId });
                        }

                        _mediatr.Post(new CreateUserPointsCommand(request.RequestUser) { CreatedOn = DateTimeOffset.UtcNow, Kind = PointKind.PurchaseOrder, UserId = order.User.Id });
                        if (order.Donate > 0)
                            _mediatr.Post(new CreateDonationCommand(request.RequestUser) { OrderId = order.Id });

                        return Ok(purchaseOrderIds.AsEnumerable());
                    }
                }
                catch (Exception e)
                {
                    _mediatr.Post(new ConfirmOrderFailedEvent(request.RequestUser) { OrderId = request.OrderId, Message = e.Message });
                    throw;
                }
            });
        }
    }
}
