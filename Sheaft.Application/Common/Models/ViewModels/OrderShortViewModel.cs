using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models.ViewModels
{
    public class OrderShortViewModel
    {
        public Guid Id { get; set; }
        public string Identifier { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
    }
}
