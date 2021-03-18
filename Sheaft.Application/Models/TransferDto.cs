namespace Sheaft.Application.Models
{
    public class TransferDto : TransactionDto
    {
        public UserDto DebitedUser { get; set; }
        public UserDto CreditedUser { get; set; }
        public PurchaseOrderDto PurchaseOrder { get; set; }
    }
}