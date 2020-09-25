namespace Sheaft.Application.Models
{
    public class PayinRefundDto : RefundDto
    {
        public UserProfileDto DebitedUser { get; set; }
        public TransactionDto Payin { get; set; }
    }
}