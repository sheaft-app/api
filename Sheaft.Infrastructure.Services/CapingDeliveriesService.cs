using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interop;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain.Models;
using Sheaft.Exceptions;
using Sheaft.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Infrastructure.Services
{
    public class CapingDeliveriesService : BaseService, ICapingDeliveriesService
    {
        private readonly CloudStorageAccount _cloudStorageAccount;
        private readonly StorageOptions _storageOptions;

        public CapingDeliveriesService(
            CloudStorageAccount cloudStorageAccount,
            IOptionsSnapshot<StorageOptions> storageOptions,
            ILogger<CapingDeliveriesService> logger) : base(logger)
        {
            _storageOptions = storageOptions.Value;
            _cloudStorageAccount = cloudStorageAccount;
        }        

        public async Task<Result<bool>> ValidateCapedDeliveriesAsync(IReadOnlyCollection<OrderDelivery> orderDeliveries, CancellationToken token)
        {
            if (orderDeliveries.All(d => !d.DeliveryMode.MaxPurchaseOrdersPerTimeSlot.HasValue))
                return Ok(true);

            var results = await GetCapingDeliveriesAsync(
                orderDeliveries.Where(d => d.DeliveryMode.MaxPurchaseOrdersPerTimeSlot.HasValue).Select(d =>
                    new Tuple<Guid, Guid, DeliveryHourDto>(
                        d.DeliveryMode.Producer.Id,
                        d.DeliveryMode.Id,
                        new DeliveryHourDto
                        {
                            Day = d.ExpectedDelivery.ExpectedDeliveryDate.DayOfWeek,
                            ExpectedDeliveryDate = d.ExpectedDelivery.ExpectedDeliveryDate,
                            From = d.ExpectedDelivery.From,
                            To = d.ExpectedDelivery.To,
                        })
                    ), token);

            if (!results.Success)
                return Failed<bool>(results.Exception);

            foreach (var orderDelivery in orderDeliveries)
            {
                var delivery = results.Data.FirstOrDefault(d => d.DeliveryId == orderDelivery.Id 
                    && d.ExpectedDate.Year == orderDelivery.ExpectedDelivery.ExpectedDeliveryDate.Year 
                    && d.ExpectedDate.Month == orderDelivery.ExpectedDelivery.ExpectedDeliveryDate.Month 
                    && d.ExpectedDate.Day == orderDelivery.ExpectedDelivery.ExpectedDeliveryDate.Day
                    && d.From == orderDelivery.ExpectedDelivery.From
                    && d.To == orderDelivery.ExpectedDelivery.To);

                if (delivery == null)
                    continue;

                if (delivery.Count >= orderDelivery.DeliveryMode.MaxPurchaseOrdersPerTimeSlot)
                    return Failed<bool>(new ValidationException(MessageKind.Order_CannotPay_Delivery_Max_PurchaseOrders_Reached, orderDelivery.DeliveryMode.Producer.Name, $"le {orderDelivery.ExpectedDelivery.ExpectedDeliveryDate:dd/MM/yyyy} entre {orderDelivery.ExpectedDelivery.From:hh\\hmm} et {orderDelivery.ExpectedDelivery.To:hh\\hmm}", orderDelivery.DeliveryMode.MaxPurchaseOrdersPerTimeSlot));
            }

            return Ok(true);
        }

        public async Task<Result<IEnumerable<CapingDeliveryDto>>> GetCapingDeliveriesAsync(IEnumerable<Tuple<Guid, Guid, DeliveryHourDto>> deliveries, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var table = _cloudStorageAccount.CreateCloudTableClient().GetTableReference(_storageOptions.Tables.CapingDeliveries);
                await table.CreateIfNotExistsAsync(token);

                var tasks = new List<Task<TableResult>>();
                foreach (var delivery in deliveries)
                    tasks.Add(table.ExecuteAsync(TableOperation.Retrieve<CapingDeliveryTableEntity>(
                            GetPartitionKey(delivery.Item1, delivery.Item2, delivery.Item3.ExpectedDeliveryDate),
                            GetRowKey(delivery.Item3.From, delivery.Item3.To)
                        ), token));

                var capingDeliveries = new List<CapingDeliveryDto>();
                var results = await Task.WhenAll(tasks);
                foreach (var result in results)
                {
                    if (result == null || result.HttpStatusCode != 404 && result.HttpStatusCode > 400)
                        continue;

                    if (result.Result is CapingDeliveryTableEntity capingDelivery && capingDelivery != null)
                        capingDeliveries.Add(new CapingDeliveryDto
                        {
                            PartitionKey = capingDelivery.PartitionKey,
                            RowKey = capingDelivery.RowKey,
                            Count = capingDelivery.Count,
                            DeliveryId = capingDelivery.DeliveryId,
                            ProducerId = capingDelivery.ProducerId,
                            ExpectedDate = capingDelivery.ExpectedDate,
                            From = TimeSpan.FromSeconds(capingDelivery.From),
                            To = TimeSpan.FromSeconds(capingDelivery.To)
                        });
                }

                return Ok(capingDeliveries.AsEnumerable());
            });
        }

        public async Task<Result<CapingDeliveryDto>> GetCapingDeliveryAsync(Guid producerId, Guid deliveryId, DateTimeOffset expectedDeliveryDate, TimeSpan from, TimeSpan to, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var table = _cloudStorageAccount.CreateCloudTableClient().GetTableReference(_storageOptions.Tables.CapingDeliveries);
                await table.CreateIfNotExistsAsync(token);

                string partitionKey = GetPartitionKey(producerId, deliveryId, expectedDeliveryDate);
                string rowkey = GetRowKey(from, to);

                var result = await table.ExecuteAsync(TableOperation.Retrieve<CapingDeliveryTableEntity>(partitionKey, rowkey), token);
                if (result == null || result.HttpStatusCode >= 400)
                    return Failed<CapingDeliveryDto>(new Exception($"{result.HttpStatusCode}-{result.SessionToken}"));

                var capingDelivery = result.Result as CapingDeliveryTableEntity;
                if (capingDelivery == null)
                    return Ok(new CapingDeliveryDto
                    {
                        PartitionKey = partitionKey,
                        RowKey = rowkey,
                        Count = 0,
                        DeliveryId = deliveryId,
                        ProducerId = producerId,
                        ExpectedDate = expectedDeliveryDate,
                        From = from,
                        To = to
                    });

                return Ok(new CapingDeliveryDto
                {
                    PartitionKey = capingDelivery.PartitionKey,
                    RowKey = capingDelivery.RowKey,
                    Count = capingDelivery.Count,
                    DeliveryId = capingDelivery.DeliveryId,
                    ProducerId = capingDelivery.ProducerId,
                    ExpectedDate = capingDelivery.ExpectedDate,
                    From = TimeSpan.FromSeconds(capingDelivery.From),
                    To = TimeSpan.FromSeconds(capingDelivery.To)
                });
            });
        }

        public async Task<Result<bool>> IncreaseProducerDeliveryCountAsync(Guid producerId, Guid deliveryId, DateTimeOffset expectedDeliveryDate, TimeSpan from, TimeSpan to, int maxPurchaseOrders, CancellationToken token)
        {
            return await UpdateProducerDeliveryCountAsync(producerId, deliveryId, expectedDeliveryDate, from, to, maxPurchaseOrders, +1, token);
        }

        public async Task<Result<bool>> DecreaseProducerDeliveryCountAsync(Guid producerId, Guid deliveryId, DateTimeOffset expectedDeliveryDate, TimeSpan from, TimeSpan to, int maxPurchaseOrders, CancellationToken token)
        {
            return await UpdateProducerDeliveryCountAsync(producerId, deliveryId, expectedDeliveryDate, from, to, maxPurchaseOrders, -1, token);
        }

        private async Task<Result<bool>> UpdateProducerDeliveryCountAsync(Guid producerId, Guid deliveryId, DateTimeOffset expectedDeliveryDate, TimeSpan from, TimeSpan to, int maxPurchaseOrders, int change, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var table = _cloudStorageAccount.CreateCloudTableClient().GetTableReference(_storageOptions.Tables.CapingDeliveries);
                await table.CreateIfNotExistsAsync(token);

                var concurrentUpdateError = false;
                string partitionKey = GetPartitionKey(producerId, deliveryId, expectedDeliveryDate);
                string rowkey = GetRowKey(from, to);

                var retry = 0;

                do
                {
                    try
                    {
                        var tableResults = await table.ExecuteAsync(TableOperation.Retrieve<CapingDeliveryTableEntity>(partitionKey, rowkey), token);
                        var results = (CapingDeliveryTableEntity)tableResults.Result;
                        if (results != null)
                        {
                            results.Count += change;
                            if (!_storageOptions.RequireEtag)
                                results.ETag = "*";

                            await table.ExecuteAsync(TableOperation.Replace(results), token);
                        }
                        else
                        {
                            await table.ExecuteAsync(TableOperation.Insert(new CapingDeliveryTableEntity
                            {
                                PartitionKey = partitionKey,
                                RowKey = rowkey,
                                Count = change < 0 ? 0 : 1,
                                ProducerId = producerId,
                                DeliveryId = deliveryId,
                                ExpectedDate = expectedDeliveryDate,
                                From = from.TotalSeconds,
                                To = to.TotalSeconds,
                            }), token);
                        }

                        concurrentUpdateError = false;
                    }
                    catch (StorageException e)
                    {
                        retry++;
                        if (retry <= 10 && (e.RequestInformation.HttpStatusCode == 412 || e.RequestInformation.HttpStatusCode == 409))
                            concurrentUpdateError = true;
                        else
                            throw;
                    }
                } while (concurrentUpdateError);

                return Ok(true);
            });
        }

        private static string GetRowKey(TimeSpan from, TimeSpan to)
        {
            return $"{from.TotalSeconds}-{to.TotalSeconds}";
        }

        private static string GetPartitionKey(Guid producerId, Guid deliveryId, DateTimeOffset expectedDeliveryDate)
        {
            return $"{producerId}-{deliveryId}_{expectedDeliveryDate.Year}{expectedDeliveryDate.Month}{expectedDeliveryDate.Day}";
        }

        private class CapingDeliveryTableEntity : TableEntity
        {
            public int Count { get; set; }
            public Guid ProducerId { get; set; }
            public Guid DeliveryId { get; set; }
            public DateTimeOffset ExpectedDate { get; set; }
            public double From { get; set; }
            public double To { get; set; }
        }
    }
}