using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Web.Manage.Models
{
    public class OrderShortViewModel
    {
        public Guid Id { get; set; }
        public decimal? Donate { get; set; }
        public string Identifier { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalFees { get; set; }
        public OrderStatus Status { get; set; }
    }
}
