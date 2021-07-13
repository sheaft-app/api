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
using Sheaft.Domain.Extensions;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Business
{
    public class PickingService : SheaftService, IPickingService
    {
        private readonly IDbContextFactory<QueryDbContext> _contextFactory;

        public PickingService(
            IDbContextFactory<QueryDbContext> contextFactory,
            ILogger<PickingService> logger) : base(logger)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<AvailablePickingDto>> GetAvailablePickingsAsync(Guid producerId,
            bool includePendingPurchaseOrders, CancellationToken token)
        {
            var context = _contextFactory.CreateDbContext();
            var purchaseOrders = await context.PurchaseOrders
                .Where(po =>
                    (po.Status == PurchaseOrderStatus.Accepted || includePendingPurchaseOrders &&
                        po.Status == PurchaseOrderStatus.Waiting)
                    && po.ProducerId == producerId
                    && !po.PickingId.HasValue)
                .ToListAsync(token);

           
            var availablePickings = new List<AvailablePickingDto>();
            foreach (var groupedPurchaseOrderByDate in purchaseOrders
                .GroupBy(p => p.ExpectedDelivery.ExpectedDeliveryDate))
            {
                AvailablePickingDto picking = null;
                foreach (var purchaseOrderClients in groupedPurchaseOrderByDate.GroupBy(po => po.ClientId))
                {
                    AvailableClientPickingDto client = null;
                    foreach (var purchaseOrder in purchaseOrderClients)
                    {
                        picking ??= new AvailablePickingDto
                        {
                            Name = purchaseOrder.ExpectedDelivery.Name,
                            ExpectedDeliveryDate = purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate,
                            Clients = new List<AvailableClientPickingDto>()
                        };

                        if (client == null)
                        {
                            client = new AvailableClientPickingDto
                            {
                                Id = purchaseOrder.ClientId,
                                Name = purchaseOrder.SenderInfo.Name,
                                PurchaseOrders = new List<AvailablePurchaseOrderDto>()
                            };

                            picking.Clients.Add(client);
                        }

                        client.PurchaseOrders.Add(new AvailablePurchaseOrderDto
                        {
                            Id = purchaseOrder.Id,
                            Status = purchaseOrder.Status,
                            Address = purchaseOrder.ExpectedDelivery.Address?.ToString() ??
                                      purchaseOrder.SenderInfo.Address,
                            Client = purchaseOrder.SenderInfo.Name,
                            ClientId = purchaseOrder.ClientId,
                            Reference = purchaseOrder.Reference.AsPurchaseOrderIdentifier(),
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

                if (picking == null) 
                    continue;
                
                picking.ClientsCount = picking.Clients.Count;
                picking.PurchaseOrdersCount = picking.Clients.Sum(c => c.PurchaseOrdersCount);
                picking.ProductsCount = picking.Clients.Sum(c => c.PurchaseOrders.Sum(po => po.ProductsCount));
                
                availablePickings.Add(picking);
            }

            return availablePickings;
        }
    }
}