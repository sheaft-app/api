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
using Sheaft.Application.Interfaces.Business;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Order.Commands
{
    public class UpdateConsumerOrderCommand : Command
    {
        protected UpdateConsumerOrderCommand()
        {
            
        }
        [JsonConstructor]
        public UpdateConsumerOrderCommand(RequestUser requestUser) : base(requestUser)
        {
            UserId = requestUser.IsAuthenticated ? requestUser.Id : (Guid?) null;
        }

        public Guid? UserId { get; set; }
        public Guid OrderId { get; set; }
        public DonationKind Donation { get; set; }
        public IEnumerable<ResourceIdQuantityInputDto> Products { get; set; }
        public IEnumerable<ProducerExpectedDeliveryInputDto> ProducersExpectedDeliveries { get; set; }

        public override void SetRequestUser(RequestUser user)
        {
            base.SetRequestUser(user);            
            UserId = user.IsAuthenticated ? user.Id : (Guid?) null;
        }
    }

    public class UpdateConsumerOrderCommandHandler : CommandsHandler,
        IRequestHandler<UpdateConsumerOrderCommand, Result>
    {
        private readonly IOrderService _orderService;

        public UpdateConsumerOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IOrderService orderService,
            ILogger<UpdateConsumerOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _orderService = orderService;
        }

        public async Task<Result> Handle(UpdateConsumerOrderCommand request, CancellationToken token)
        {
            var entity = await _context.Orders.SingleAsync(e => e.Id == request.OrderId, token);
            if (entity.User != null && entity.UserId != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);
            
            if(entity.User != null && request.UserId.HasValue && request.UserId.Value != entity.UserId)
                return Failure(MessageKind.Conflict);

            if (entity.User == null && request.UserId.HasValue && request.UserId != Guid.Empty)
            {
                var user = await _context.Users.SingleAsync(e => e.Id == request.UserId.Value, token);
                entity.AssignToUser(user);
            }

            var productIds = request.Products.Select(p => p.Id);
            var cartProductsResult = await _orderService.GetCartProductsAsync(productIds, request.Products, token);
            if (!cartProductsResult.Succeeded)
                return Failure<Guid>(cartProductsResult);

            var deliveryIds = request.ProducersExpectedDeliveries?.Select(p => p.DeliveryModeId) ?? new List<Guid>();
            var cartDeliveriesResult = await _orderService.GetCartDeliveriesAsync(request.ProducersExpectedDeliveries, deliveryIds,
                cartProductsResult.Data, token);
            if (!cartDeliveriesResult.Succeeded)
                return Failure<Guid>(cartDeliveriesResult);

            entity.SetProducts(cartProductsResult.Data);
            entity.SetDeliveries(cartDeliveriesResult.Data);
            entity.SetDonation(request.Donation);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
