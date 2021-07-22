using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Business;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
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
        protected CreateBusinessOrderCommand()
        {
        }

        [JsonConstructor]
        public CreateBusinessOrderCommand(RequestUser requestUser) : base(requestUser)
        {
            UserId = RequestUser.Id;
        }

        public Guid UserId { get; set; }
        public IEnumerable<ResourceIdQuantityInputDto> Products { get; set; }
        public IEnumerable<ProducerExpectedDeliveryInputDto> ProducersExpectedDeliveries { get; set; }

        public override void SetRequestUser(RequestUser user)
        {
            base.SetRequestUser(user);
            UserId = RequestUser.Id;
        }
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
            var user = await _context.Users.SingleAsync(e => e.Id == request.UserId, token);
            if (user.Id != request.RequestUser.Id)
                return Failure<IEnumerable<Guid>>("Vous n'êtes pas autorisé à accéder à cette ressource.");
            
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
                    return Failure<IEnumerable<Guid>>(
                        $"Certains produits ne sont pas disponible : {string.Join(";", invalidProductIds)}");

                var cartProducts = new List<Tuple<Domain.Product, Guid, int>>();
                var deliveryIds = request.ProducersExpectedDeliveries.Select(p => p.DeliveryModeId);
                var agreements = await _context.Agreements
                    .Where(a => a.StoreId == request.UserId && a.DeliveryModeId.HasValue &&
                                deliveryIds.Contains(a.DeliveryModeId.Value) && a.Status == AgreementStatus.Accepted)
                    .ToListAsync(token);

                Result<IEnumerable<Guid>> catalogResult = null;
                foreach (var product in products)
                {
                    var agreement = agreements.SingleOrDefault(a => a.ProducerId == product.ProducerId);
                    if (agreement?.CatalogId == null)
                    {
                        catalogResult = Failure<IEnumerable<Guid>>("Ce produit n'est pas disponible dans votre partenariat.");
                        break;
                    }

                    if (agreement.Catalog.Products.All(p => p.ProductId != product.Id))
                        invalidProductIds.Add(product.Id.ToString("N"));

                    cartProducts.Add(new Tuple<Domain.Product, Guid, int>(product, agreement.CatalogId.Value,
                        request.Products.Where(p => p.Id == product.Id).Sum(c => c.Quantity ?? 0)));
                }

                if (catalogResult is {Succeeded: false})
                    return catalogResult;

                if (invalidProductIds.Any())
                    return Failure<IEnumerable<Guid>>($"Certains produits ne sont pas disponible : {string.Join(";", invalidProductIds)}");

                var deliveries = deliveryIds.Any()
                    ? await _context.DeliveryModes.Where(d => deliveryIds.Contains(d.Id)).ToListAsync(token)
                    : new List<Domain.DeliveryMode>();
                var cartDeliveries = new List<Tuple<Domain.DeliveryMode, DateTimeOffset, string>>();
                Result<IEnumerable<Guid>> deliveryResult = null;
                foreach (var delivery in deliveries)
                {
                    var cartDelivery =
                        request.ProducersExpectedDeliveries.FirstOrDefault(ped => ped.DeliveryModeId == delivery.Id);

                    if (delivery.Closings.Any(c =>
                        cartDelivery.ExpectedDeliveryDate >= c.ClosedFrom &&
                        cartDelivery.ExpectedDeliveryDate <= c.ClosedTo))
                    {
                        deliveryResult = Failure<IEnumerable<Guid>>($"Le créneau {delivery.Name} du producteur {delivery.Producer.Name} est fermé à la date selectionnée");
                        break;
                    }

                    if (delivery.Producer.Closings.Any(c =>
                        cartDelivery.ExpectedDeliveryDate >= c.ClosedFrom &&
                        cartDelivery.ExpectedDeliveryDate <= c.ClosedTo))
                    {
                        deliveryResult = Failure<IEnumerable<Guid>>($"Le producteur {delivery.Producer.Name} est fermé à la date selectionnée");
                        break;
                    }

                    cartDeliveries.Add(new Tuple<Domain.DeliveryMode, DateTimeOffset, string>(delivery,
                        cartDelivery.ExpectedDeliveryDate, cartDelivery.Comment));
                }

                if (deliveryResult is {Succeeded: false})
                    return deliveryResult;

                if (!cartDeliveries.Any())
                    return Failure<IEnumerable<Guid>>("Les crénaux de livraisons doivent être selectionnés.");

                var order = new Domain.Order(Guid.NewGuid(), DonationKind.None, cartProducts,
                    _pspOptions.FixedAmount, _pspOptions.Percent, _pspOptions.VatPercent, user);

                order.SetDeliveries(cartDeliveries);
                order.SetStatus(OrderStatus.Confirmed);

                var validatedDeliveries = await _deliveryService.ValidateCapedDeliveriesAsync(order.Deliveries, token);
                if (!validatedDeliveries.Succeeded)
                    return Failure<IEnumerable<Guid>>(validatedDeliveries);

                await _context.AddAsync(order, token);
                await _context.SaveChangesAsync(token);

                var purchaseOrderIds = new List<Result<Guid>>();
                var producerIds = order.Products.Select(p => p.ProducerId).Distinct();
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

                if (purchaseOrderIds.Any(r => !r.Succeeded))
                    return Failure<IEnumerable<Guid>>(purchaseOrderIds.First(r => !r.Succeeded));

                await transaction.CommitAsync(token);
                return Success(purchaseOrderIds.Select(p => p.Data));
            }
        }
    }
}