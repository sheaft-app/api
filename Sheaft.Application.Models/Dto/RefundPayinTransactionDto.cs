namespace Sheaft.Application.Models
{
    public class RefundPayinTransactionDto : RefundTransactionDto
    {
        public UserProfileDto DebitedUser { get; set; }
    }
}