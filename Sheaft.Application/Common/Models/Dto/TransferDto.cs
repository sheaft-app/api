namespace Sheaft.Application.Models
{
    public class TransferDto : TransactionDto
    {
        public UserProfileDto DebitedUser { get; set; }
        public UserProfileDto CreditedUser { get; set; }
        public PurchaseOrderDto PurchaseOrder { get; set; }
    }
}