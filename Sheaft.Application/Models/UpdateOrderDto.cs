using System;

namespace Sheaft.Application.Models
{
    public class UpdateOrderDto : CreateOrderDto
    {
        public Guid Id { get; set; }
    }
}