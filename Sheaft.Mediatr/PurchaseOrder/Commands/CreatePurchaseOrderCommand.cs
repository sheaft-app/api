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
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.PurchaseOrder.Commands
{
    public class CreatePurchaseOrderCommand : Command<Guid>
    {
        protected CreatePurchaseOrderCommand()
        {
        }

        [JsonConstructor]
        public CreatePurchaseOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ClientId { get; set; }
        public Guid ProducerId { get; set; }
        public Guid DeliveryModeId { get; set; }
        public DateTimeOffset ExpectedDeliveryDate { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
        public IEnumerable<ResourceIdQuantityInputDto> Products { get; set; }
        public bool SkipNotification { get; set; }
        public string Comment { get; set; }

        public override void SetRequestUser(RequestUser user)
        {
            base.SetRequestUser(user);
            ProducerId = user.Id;
        }
    }

    public class CreatePurchaseOrderCommandHandler : CommandsHandler,
        IRequestHandler<CreatePurchaseOrderCommand, Result<Guid>>
    {
        private readonly IIdentifierService _identifierService;
        private readonly ITableService _tableService;

        public CreatePurchaseOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IIdentifierService identifierService,
            ITableService tableService,
            ILogger<CreatePurchaseOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _identifierService = identifierService;
            _tableService = tableService;
        }

        public async Task<Result<Guid>> Handle(CreatePurchaseOrderCommand request, CancellationToken token)
        {
            var producer = await _context.Producers.SingleAsync(e => e.Id == request.ProducerId, token);
            var client = await _context.Users.SingleAsync(e => e.Id == request.ClientId, token);
            var deliveryMode =
                await _context.DeliveryModes.SingleAsync(
                    d => d.Id == request.DeliveryModeId && d.ProducerId == producer.Id, token);

            if (client.Kind == ProfileKind.Consumer)
                return Failure<Guid>(
                    "Cette fonction ne peux être utilisée que pour créer des commandes pour les professionels.");

            var productsQuantity = new List<KeyValuePair<Domain.Product, int>>();
            var productIds = request.Products.Select(p => p.Id).Distinct();
            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id) && p.ProducerId == producer.Id)
                .ToListAsync(token);

            Domain.Catalog catalog = null;
            var agreement = await _context.Agreements.SingleAsync(
                a => a.StoreId == client.Id && a.ProducerId == producer.Id && a.Status == AgreementStatus.Accepted,
                token);
            if (agreement != null)
                catalog = agreement.Catalog;

            catalog ??= await _context.Catalogs.SingleAsync(
                c => c.ProducerId == producer.Id && c.Kind == CatalogKind.Stores && c.Available && c.IsDefault, token);

            foreach (var product in products)
            {
                var quantity = request.Products.Where(p => p.Id == product.Id).Sum(p => p.Quantity);
                productsQuantity.Add(new KeyValuePair<Domain.Product, int>(product, quantity));
            }

            var resultIdentifier =
                await _identifierService.GetNextPurchaseOrderReferenceAsync(request.ProducerId, token);
            if (!resultIdentifier.Succeeded)
                return Failure<Guid>(resultIdentifier);

            var purchaseOrder = new Domain.PurchaseOrder(Guid.NewGuid(), resultIdentifier.Data,
                PurchaseOrderStatus.Accepted, request.ExpectedDeliveryDate, request.From, request.To, deliveryMode,
                producer, client, productsQuantity, catalog, request.SkipNotification, request.Comment);

            await _context.AddAsync(purchaseOrder, token);
            await _context.SaveChangesAsync(token);

            if (deliveryMode.MaxPurchaseOrdersPerTimeSlot.HasValue)
                await _tableService.IncreaseProducerDeliveryCountAsync(producer.Id, deliveryMode.Id,
                    request.ExpectedDeliveryDate, request.From,
                    request.To, deliveryMode.MaxPurchaseOrdersPerTimeSlot.Value, token);

            return Success(purchaseOrder.Id);
        }
    }
}