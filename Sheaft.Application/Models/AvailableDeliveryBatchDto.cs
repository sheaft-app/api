using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class AvailableDeliveryBatchDto
    {
        public string Name { get; set; }
        public DateTimeOffset ExpectedDeliveryDate { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
        public DayOfWeek Day { get; set; }
        public int ClientsCount { get; set; }
        public int ProductsCount { get; set; }
        public int PurchaseOrdersCount { get; set; }
        public List<AvailableClientDeliveryDto> Clients { get; set; }
    }
}