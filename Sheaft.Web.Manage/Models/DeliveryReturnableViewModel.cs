using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Web.Manage.Models
{
    public class DeliveryReturnableViewModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set;}
        public DateTimeOffset? UpdatedOn { get; set;}
        public int Quantity { get; set; }
        public ReturnableKind Kind { get; set; }
        public string Name { get; set; }
        public decimal Vat { get; set; }
        public decimal UnitWholeSalePrice { get; set; }
        public decimal UnitVatPrice { get; set; }
        public decimal UnitOnSalePrice { get; set; }
        public decimal TotalWholeSalePrice { get; set; }
        public decimal TotalVatPrice { get; set; }
        public decimal TotalOnSalePrice { get; set; }
        public Guid ReturnableId { get; set;}
        public Guid DeliveryId { get; set;}
    }
}