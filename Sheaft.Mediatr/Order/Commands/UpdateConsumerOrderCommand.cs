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
using Microsoft.EntityFrameworkCore;
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
            UserId = requestUser.IsAuthenticated() ? requestUser.Id : (Guid?) null;
        }

        public Guid? UserId { get; set; }
        public Guid OrderId { get; set; }
        public DonationKind Donation { get; set; }
        public IEnumerable<ResourceIdQuantityInputDto> Products { get; set; }
        public IEnumerable<ProducerExpectedDeliveryInputDto> ProducersExpectedDeliveries { get; set; }

        public override void SetRequestUser(RequestUser user)
        {
            base.SetRequestUser(user);            
            UserId = user.IsAuthenticated() ? user.Id : (Guid?) null;
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
                return Failure("Vous n'êtes pas autorisé à accéder à cette ressource.");
            
            if(entity.User != null && request.UserId.HasValue && request.UserId.Value != entity.UserId)
                return Failure("Ce panier est déjà rattaché à un autre utilisateur.");

            if (entity.User == null && request.UserId.HasValue && request.UserId != Guid.Empty)
            {
                var user = await _context.Users.SingleAsync(e => e.Id == request.UserId.Value, token);
                entity.AssignToUser(user);
            }

            var cartProductsResult = await _orderService.GetCartProductsAsync(request.Products, token);
            if (!cartProductsResult.Succeeded)
                return Failure<Guid>(cartProductsResult);

            var cartDeliveriesResult = await _orderService.GetCartDeliveriesAsync(request.ProducersExpectedDeliveries,
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
