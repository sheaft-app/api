namespace Sheaft.Application.Common.Models.Dto
{
    public class TransferDto : TransactionDto
    {
        public UserProfileDto DebitedUser { get; set; }
        public UserProfileDto CreditedUser { get; set; }
        public PurchaseOrderDto PurchaseOrder { get; set; }
    }
}