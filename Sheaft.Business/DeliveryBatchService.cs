using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Interfaces.Business;
using Sheaft.Application.Interfaces.Infrastructure;
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

        public async Task<IEnumerable<AvailableDeliveryBatchDto>> GetAvailableDeliveryBatchsAsync(Guid producerId, bool includeProcessingPurchaseOrders,
            CancellationToken token)
        {
            var purchaseOrders = await _contextFactory.CreateDbContext().PurchaseOrders
                .Where(po => 
                    (po.Status == PurchaseOrderStatus.Completed || includeProcessingPurchaseOrders && po.Status == PurchaseOrderStatus.Processing) 
                    && po.ProducerId == producerId
                    && (po.Delivery.Kind == DeliveryKind.ProducerToConsumer || po.Delivery.Kind == DeliveryKind.ProducerToStore)
                    && po.Delivery.Status == DeliveryStatus.Waiting)
                .Include(po => po.Delivery)
                .ToListAsync(token);

            var availableDeliveryBatchs = new List<AvailableDeliveryBatchDto>();
            foreach (var groupedPurchaseOrder in purchaseOrders.GroupBy(po => po.Delivery.ExpectedDeliveryDate))
            {
                var delivery = groupedPurchaseOrder.First().Delivery;
                var deliveryBatch = new AvailableDeliveryBatchDto
                {
                    Day = delivery.Day,
                    From = delivery.From,
                    To = delivery.To,
                    Name = delivery.Name,
                    ExpectedDeliveryDate = delivery.ExpectedDeliveryDate,
                    PurchaseOrders = new List<AvailablePurchaseOrderDto>()
                };
                foreach (var purchaseOrder in groupedPurchaseOrder)
                {
                    deliveryBatch.PurchaseOrders.Add(new AvailablePurchaseOrderDto
                    {
                        Id = purchaseOrder.Id,
                        Status = purchaseOrder.Status,
                        Address = purchaseOrder.Delivery.Address?.ToString() ?? purchaseOrder.SenderInfo.Address,
                        Client = purchaseOrder.SenderInfo.Name,
                        Reference = purchaseOrder.Reference,
                        LinesCount = purchaseOrder.LinesCount,
                        ProductsCount = purchaseOrder.ProductsCount,
                        ReturnablesCount = purchaseOrder.ReturnablesCount,
                        TotalOnSalePrice = purchaseOrder.TotalOnSalePrice,
                        TotalWholeSalePrice = purchaseOrder.TotalWholeSalePrice,
                        TotalWeight = purchaseOrder.TotalWeight,
                    });
                }

                deliveryBatch.PurchaseOrdersCount = deliveryBatch.PurchaseOrders.Count;
                availableDeliveryBatchs.Add(deliveryBatch);
            }

            return availableDeliveryBatchs;
        }
    }
}