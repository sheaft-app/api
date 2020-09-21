namespace Sheaft.Application.Models
{
    public class PayoutTransactionDto : TransactionDto
    {
        public UserProfileDto DebitedUser { get; set; }
    }
}