using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Interfaces.Business;
using Sheaft.Application.Models;
using Sheaft.Application.Services;
using Sheaft.Domain.Enum;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Business
{
    public class DeliveryBatchService : SheaftService, IDeliveryBatchService
    {
        private readonly IDbContextFactory<QueryDbContext> _contextFactory;

        public DeliveryBatchService(
            IDbContextFactory<QueryDbContext> contextFactory,
            ILogger<DeliveryBatchService> logger) : base(logger)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<AvailableDeliveryBatchDto>> GetAvailableDeliveryBatchesAsync(Guid producerId,
            bool includeProcessingPurchaseOrders, CancellationToken token)
        {
            var context = _contextFactory.CreateDbContext();
            var purchaseOrders = await context.PurchaseOrders
                .Where(po =>
                    (po.Status == PurchaseOrderStatus.Completed || includeProcessingPurchaseOrders &&
                        po.Status == PurchaseOrderStatus.Processing)
                    && po.ProducerId == producerId
                    && !po.DeliveryId.HasValue
                    && po.ExpectedDelivery.Kind == DeliveryKind.ProducerToStore)
                .ToListAsync(token);

            var storeIds = purchaseOrders.Select(po => po.ClientId);
            var agreements = await context.Agreements
                .Where(a => storeIds.Contains(a.StoreId) && a.Status == AgreementStatus.Accepted && a.ProducerId == producerId)
                .ToListAsync(token);

            var maxPosition = agreements.Count;
            var availableDeliveryBatchs = new List<AvailableDeliveryBatchDto>();
            foreach (var groupedPurchaseOrderByDate in purchaseOrders
                .GroupBy(p => p.ExpectedDelivery.ExpectedDeliveryDate))
            {
                AvailableDeliveryBatchDto deliveryBatch = null;
                foreach (var purchaseOrderClients in groupedPurchaseOrderByDate.GroupBy(po => po.ClientId))
                {
                    AvailableClientDeliveryDto client = null;
                    foreach (var purchaseOrder in purchaseOrderClients)
                    {
                        deliveryBatch ??= new AvailableDeliveryBatchDto
                        {
                            Name = purchaseOrder.ExpectedDelivery.Name,
                            Day = purchaseOrder.ExpectedDelivery.Day,
                            From = purchaseOrder.ExpectedDelivery.From,
                            To = purchaseOrder.ExpectedDelivery.To,
                            ExpectedDeliveryDate = purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate,
                            Clients = new List<AvailableClientDeliveryDto>()
                        };

                        if (client == null)
                        {
                            var position = agreements.FirstOrDefault(a => a.StoreId == purchaseOrder.ClientId)?.Position ?? -1;
                            if (position < 0)
                                position = maxPosition++;

                            if (deliveryBatch.Clients.Any(c => c.Position == position))
                            {
                                while(deliveryBatch.Clients.Any(c => c.Position == position))
                                    position++;

                                if (position > maxPosition)
                                    maxPosition = position;
                            }

                            client = new AvailableClientDeliveryDto
                            {
                                Id = purchaseOrder.ClientId,
                                Name = purchaseOrder.SenderInfo.Name,
                                PurchaseOrders = new List<AvailablePurchaseOrderDto>(),
                                Position = position
                            };

                            deliveryBatch.Clients.Add(client);
                        }

                        client.PurchaseOrders.Add(new AvailablePurchaseOrderDto
                        {
                            Id = purchaseOrder.Id,
                            Status = purchaseOrder.Status,
                            Address = purchaseOrder.ExpectedDelivery.Address?.ToString() ??
                                      purchaseOrder.SenderInfo.Address,
                            Client = purchaseOrder.SenderInfo.Name,
                            ClientId = purchaseOrder.ClientId,
                            Reference = purchaseOrder.Reference,
                            LinesCount = purchaseOrder.LinesCount,
                            ProductsCount = purchaseOrder.ProductsCount,
                            ReturnablesCount = purchaseOrder.ReturnablesCount,
                            TotalOnSalePrice = purchaseOrder.TotalOnSalePrice,
                            TotalWholeSalePrice = purchaseOrder.TotalWholeSalePrice,
                            TotalWeight = purchaseOrder.TotalWeight,
                            ExpectedDeliveryDate = purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate
                        });
                    }

                    if (client == null)
                        continue;

                    client.PurchaseOrdersCount = client.PurchaseOrders.Count;
                }

                if (deliveryBatch == null) 
                    continue;
                
                deliveryBatch.ClientsCount = deliveryBatch.Clients.Count;
                deliveryBatch.PurchaseOrdersCount = deliveryBatch.Clients.Sum(c => c.PurchaseOrdersCount);

                var currentPosition = 0;
                foreach (var client in deliveryBatch.Clients.OrderBy(c => c.Position))
                    client.Position = currentPosition++;
                
                availableDeliveryBatchs.Add(deliveryBatch);
            }

            return availableDeliveryBatchs;
        }
    }
}