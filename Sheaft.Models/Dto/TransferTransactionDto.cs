using System;

namespace Sheaft.Models.Dto
{
    public class TransferTransactionDto : BaseTransactionDto
    {
        public decimal Fees { get; set; }
        public decimal Debited { get; set; }
        public decimal Credited { get; set; }
        public UserProfileDto DebitedUser { get; set; }
        public UserProfileDto CreditedUser { get; set; }
        public PurchaseOrderDto PurchaseOrder { get; set; }
    }
}