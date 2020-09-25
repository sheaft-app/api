namespace Sheaft.Application.Models
{
    public class PayoutDto : TransactionDto
    {
        public UserProfileDto DebitedUser { get; set; }
    }
}