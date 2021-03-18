namespace Sheaft.Application.Models
{
    public class PayinRefundDto : TransactionDto
    {
        public UserDto DebitedUser { get; set; }
        public TransactionDto Payin { get; set; }
    }
}