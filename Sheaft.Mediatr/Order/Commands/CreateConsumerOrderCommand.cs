using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Business;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Options;

namespace Sheaft.Mediatr.Order.Commands
{
    public class CreateConsumerOrderCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateConsumerOrderCommand(RequestUser requestUser) : base(requestUser)
        {
            UserId = requestUser.IsAuthenticated ? requestUser.Id : (Guid?) null;
        }

        public Guid? UserId { get; set; }
        public DonationKind Donation { get; set; }
        public IEnumerable<ResourceIdQuantityDto> Products { get; set; }
        public IEnumerable<ProducerExpectedDeliveryDto> ProducersExpectedDeliveries { get; set; }
    }

    public class CreateConsumerOrderCommandHandler : CommandsHandler,
        IRequestHandler<CreateConsumerOrderCommand, Result<Guid>>
    {
        private readonly IOrderService _orderService;
        private readonly PspOptions _pspOptions;

        public CreateConsumerOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IOrderService orderService,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<CreateConsumerOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _orderService = orderService;
            _pspOptions = pspOptions.Value;
        }

        public async Task<Result<Guid>> Handle(CreateConsumerOrderCommand request, CancellationToken token)
        {
            var productIds = request.Products.Select(p => p.Id);
            var cartProductsResult = await _orderService.GetCartProductsAsync(productIds, request.Products, token);
            if (!cartProductsResult.Succeeded)
                return Failure<Guid>(cartProductsResult);

            var deliveryIds = request.ProducersExpectedDeliveries?.Select(p => p.DeliveryModeId) ?? new List<Guid>();
            var cartDeliveriesResult = await _orderService.GetCartDeliveriesAsync(request.ProducersExpectedDeliveries, deliveryIds,
                cartProductsResult.Data, token);
            if (!cartDeliveriesResult.Succeeded)
                return Failure<Guid>(cartDeliveriesResult);
            
            Domain.User user = request.UserId.HasValue && request.UserId != Guid.Empty ? await _context.GetByIdAsync<Domain.User>(request.UserId.Value, token) : null;
            var order = new Domain.Order(Guid.NewGuid(), request.Donation, cartProductsResult.Data, _pspOptions.FixedAmount,
                _pspOptions.Percent, _pspOptions.VatPercent, user);
            
            order.SetProducts(cartProductsResult.Data);
            order.SetDeliveries(cartDeliveriesResult.Data);

            await _context.AddAsync(order, token);
            await _context.SaveChangesAsync(token);

            return Success(order.Id);
        }
    }
}
