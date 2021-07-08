using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Web.Manage.Models
{
    public class TransferShortViewModel
    {
        public Guid Id { get; set; }
        public string Identifier { get; set; }
        public decimal Credited { get; set; }
        public TransactionStatus Status { get; set; }
        public ShortPurchaseOrderViewModel PurchaseOrder { get; set; }
    }
}
