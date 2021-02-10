using System;

namespace Sheaft.Application.Common.Models.Dto
{
    public class CapingDeliveryDto
    {
        public Guid ProducerId { get; set; }
        public Guid DeliveryId { get; set; }
        public DateTimeOffset ExpectedDate { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public int Count { get; set; }
    }
}