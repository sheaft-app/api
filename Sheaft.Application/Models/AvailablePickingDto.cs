using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class AvailablePickingDto
    {
        public string Name { get; set; }
        public DateTimeOffset ExpectedDeliveryDate { get; set; }
        public int ClientsCount { get; set; }
        public int PurchaseOrdersCount { get; set; }
        public int ProductsCount { get; set; }
        public List<AvailableClientPickingDto> Clients { get; set; }
    }
}