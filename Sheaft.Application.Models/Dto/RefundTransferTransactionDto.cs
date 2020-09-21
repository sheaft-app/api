namespace Sheaft.Application.Models
{
    public class RefundTransferTransactionDto : RefundTransactionDto
    {
        public UserProfileDto DebitedUser { get; set; }
        public UserProfileDto CreditedUser { get; set; }
    }
}