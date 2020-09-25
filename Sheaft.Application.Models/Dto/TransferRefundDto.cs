namespace Sheaft.Application.Models
{
    public class TransferRefundDto : RefundDto
    {
        public UserProfileDto DebitedUser { get; set; }
        public UserProfileDto CreditedUser { get; set; }
        public TransactionDto Transfer { get; set; }
    }
}