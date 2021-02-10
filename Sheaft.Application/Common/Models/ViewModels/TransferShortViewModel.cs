using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models.ViewModels
{
    public class TransferShortViewModel
    {
        public Guid Id { get; set; }
        public string Identifier { get; set; }
        public decimal Credited { get; set; }
        public TransactionStatus Status { get; set; }
        public PurchaseOrderShortViewModel PurchaseOrder { get; set; }
    }
}
