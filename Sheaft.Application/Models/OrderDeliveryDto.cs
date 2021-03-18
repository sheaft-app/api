using System;

namespace Sheaft.Application.Models
{
    public class OrderDeliveryDto
    {

        public Guid Id { get; set; }
        public string Comment { get; set; }
        public virtual ExpectedOrderDeliveryDto ExpectedDelivery { get; set; }
        public virtual DeliveryModeDto DeliveryMode { get; set; }
    }
}