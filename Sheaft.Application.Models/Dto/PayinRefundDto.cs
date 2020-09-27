namespace Sheaft.Application.Models
{
    public class PayinRefundDto : TransactionDto
    {
        public UserProfileDto DebitedUser { get; set; }
        public TransactionDto Payin { get; set; }
    }
}