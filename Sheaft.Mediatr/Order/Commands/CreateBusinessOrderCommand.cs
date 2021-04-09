﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Business;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.PurchaseOrder.Commands;
using Sheaft.Mediatr.User.Commands;
using Sheaft.Options;

namespace Sheaft.Mediatr.Order.Commands
{
    public class CreateBusinessOrderCommand : Command<IEnumerable<Guid>>
    {
        [JsonConstructor]
        public CreateBusinessOrderCommand(RequestUser requestUser) : base(requestUser)
        {
            UserId = requestUser.Id;
        }

        public Guid UserId { get; set; }
        public IEnumerable<ResourceIdQuantityDto> Products { get; set; }
        public IEnumerable<ProducerExpectedDeliveryDto> ProducersExpectedDeliveries { get; set; }
    }

    public class CreateBusinessOrderCommandHandler : CommandsHandler,
        IRequestHandler<CreateBusinessOrderCommand, Result<IEnumerable<Guid>>>
    {
        private readonly IDeliveryService _deliveryService;
        private readonly IIdentifierService _identifierService;
        private readonly PspOptions _pspOptions;

        public CreateBusinessOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IDeliveryService deliveryService,
            IIdentifierService identifierService,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<CreateBusinessOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _deliveryService = deliveryService;
            _identifierService = identifierService;
            _pspOptions = pspOptions.Value;
        }

        public async Task<Result<IEnumerable<Guid>>> Handle(CreateBusinessOrderCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var productIds = request.Products.Select(p => p.Id);
                var products = await _context.Products
                    .Where(p => productIds.Contains(p.Id))
                    .Include(p => p.Producer)
                    .Include(c => c.CatalogsPrices)
                        .ThenInclude(c => c.Catalog)
                    .ToListAsync(token);
                
                var invalidProductIds = products
                    .Where(p => 
                        p.RemovedOn.HasValue 
                        || !p.Available 
                        || p.Producer.RemovedOn.HasValue)
                    .Select(p => p.Id.ToString("N"))
                    .ToList();

                if (invalidProductIds.Any())
                    return Failure<IEnumerable<Guid>>(MessageKind.Order_CannotCreate_Some_Products_NotAvailable,
                        string.Join(";", invalidProductIds));
                
                var cartProducts = new List<Tuple<Domain.Product, Guid, int>>();
                var deliveryIds = request.ProducersExpectedDeliveries.Select(p => p.DeliveryModeId);
                var agreements = await _context.GetAsync<Domain.Agreement>(a => deliveryIds.Contains(a.Delivery.Id), token);
                foreach (var product in products)
                {
                    var agreement = agreements.SingleOrDefault(a => a.Delivery.Producer.Id == product.Producer.Id);
                    if (agreement?.Catalog == null)
                        throw SheaftException.Validation();
                    
                    if(!agreement.Catalog.Products.Any(p => p.Product.Id == product.Id))
                        invalidProductIds.Add(product.Id.ToString("N"));

                    cartProducts.Add(new Tuple<Domain.Product, Guid, int>(product, agreement.Catalog.Id, request.Products.Where(p => p.Id == product.Id).Sum(c => c.Quantity)));
                }
                
                if (invalidProductIds.Any())
                    return Failure<IEnumerable<Guid>>(MessageKind.Order_CannotCreate_Some_Products_NotAvailable,
                        string.Join(";", invalidProductIds));
                
                var deliveries = deliveryIds.Any()
                    ? await _context.GetByIdsAsync<Domain.DeliveryMode>(deliveryIds, token)
                    : new List<Domain.DeliveryMode>();
                var cartDeliveries = new List<Tuple<Domain.DeliveryMode, DateTimeOffset, string>>();
                foreach (var delivery in deliveries)
                {
                    var cartDelivery =
                        request.ProducersExpectedDeliveries.FirstOrDefault(ped => ped.DeliveryModeId == delivery.Id);
                    
                    if(delivery.Closings.Any(c => cartDelivery.ExpectedDeliveryDate >= c.ClosedFrom && cartDelivery.ExpectedDeliveryDate <= c.ClosedTo))
                        return Failure<IEnumerable<Guid>>(MessageKind.Order_CannotCreate_Delivery_Closed, delivery.Name, delivery.Producer.Name);
                    
                    if(delivery.Producer.Closings.Any(c => cartDelivery.ExpectedDeliveryDate >= c.ClosedFrom && cartDelivery.ExpectedDeliveryDate <= c.ClosedTo))
                        return Failure<IEnumerable<Guid>>(MessageKind.Order_CannotCreate_Producer_Closed, delivery.Producer.Name);
                    
                    cartDeliveries.Add(new Tuple<Domain.DeliveryMode, DateTimeOffset, string>(delivery,
                        cartDelivery.ExpectedDeliveryDate, cartDelivery.Comment));
                }

                if (!cartDeliveries.Any())
                    return Failure<IEnumerable<Guid>>(MessageKind.Order_CannotCreate_Deliveries_Required);
                
                var user = await _context.GetByIdAsync<Domain.User>(request.UserId, token);
                if (user.Id != request.RequestUser.Id)
                    throw SheaftException.Forbidden();

                var order = new Domain.Order(Guid.NewGuid(), DonationKind.None, cartProducts,
                    _pspOptions.FixedAmount, _pspOptions.Percent, _pspOptions.VatPercent, user);

                order.SetDeliveries(cartDeliveries);
                order.SetStatus(OrderStatus.Validated);

                var validatedDeliveries = await _deliveryService.ValidateCapedDeliveriesAsync(order.Deliveries, token);
                if (!validatedDeliveries.Succeeded)
                    return Failure<IEnumerable<Guid>>(validatedDeliveries.Exception);

                var referenceResult = await _identifierService.GetNextOrderReferenceAsync(token);
                if (!referenceResult.Succeeded)
                    return Failure<IEnumerable<Guid>>(referenceResult.Exception);

                order.SetReference(referenceResult.Data);

                await _context.AddAsync(order, token);
                await _context.SaveChangesAsync(token);

                var purchaseOrderIds = new List<Guid>();
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
                        return Failure<IEnumerable<Guid>>(result.Exception);

                    purchaseOrderIds.Add(result.Data);
                }

                await _context.SaveChangesAsync(token);
                await transaction.CommitAsync(token);

                _mediatr.Post(new CreateUserPointsCommand(request.RequestUser)
                {
                    CreatedOn = DateTimeOffset.UtcNow, Kind = PointKind.PurchaseOrder, UserId = order.User.Id
                });

                return Success(purchaseOrderIds.AsEnumerable());
            }
        }
    }
}
